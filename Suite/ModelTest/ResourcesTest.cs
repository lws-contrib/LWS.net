using Model;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace ModelTest;

[TestClass]
public class ResourcesTest
{
    private const string Vocabulary = "http://example.com/";

    private const string RdfType = "http://www.w3.org/1999/02/22-rdf-syntax-ns#type";

    private static readonly Uri NewUri = new(Resources.Base, "new.ttl");

    [TestMethod]
    public void ParseLeavesManifestWithoutTemplatesUnchanged()
    {
        var plain = new Graph { BaseUri = NewUri };
        new TurtleParser().Load(plain, Resources.Reader("new.ttl"));

        IGraph actual = Resources.Parse(NewUri);

        var expectedManifest = GraphAssert.Reachable(plain, ManifestNodes(plain));
        var actualManifest = GraphAssert.Reachable(actual, ManifestNodes(actual));

        Assert.IsNotEmpty(expectedManifest.Triples);
        Assert.IsTrue(expectedManifest.Equals(actualManifest));
    }

    [TestMethod]
    public void ParseExpandsCreateContainerInFromSpec()
    {
        var graph = Resources.Parse(new(Resources.Base, "fromSpec.ttl"));

        var leftovers = graph.Triples
            .Where(t => t.Predicate.ToString() is Vocabulary + "argument" or Vocabulary + "expansion"
                || t.Predicate.ToString() == RdfType && t.Object.ToString() is Vocabulary + "CreateContainer" or Vocabulary + "CreateResource" or Vocabulary + "DeleteResource" or Vocabulary + "DeleteResourceIfMatch" or Vocabulary + "PutResourceIfMatch" or Vocabulary + "PatchResourceIfMatch" or Vocabulary + "ReadContainer" or Vocabulary + "StepTemplate")
            .ToArray();
        Assert.IsEmpty(leftovers);

        var step = graph.GetTriplesWithPredicateObject(graph.CreateUriNode(new Uri(Vocabulary + "stepName")), graph.CreateLiteralNode("Create container")).Single().Subject;
        var request = Model.NewModel.Step.Wrap(step, graph).Request;

        Assert.AreEqual("POST", request.Method);
        Assert.HasCount(1, request.Headers);

        var extractor = (Model.NewModel.Extractors.HeaderExtractor)Model.NewModel.Step.Wrap(step, graph).Extract.Single();
        Assert.AreEqual("Location", extractor.HeaderName);
        Assert.AreEqual("containerLocation", extractor.To);
    }

    [TestMethod]
    public void ParseExpandsCreateResourceInFromSpec()
    {
        var graph = Resources.Parse(new(Resources.Base, "fromSpec.ttl"));

        var step = graph.GetTriplesWithPredicateObject(graph.CreateUriNode(new Uri(Vocabulary + "stepName")), graph.CreateLiteralNode("Create JSON resource")).Single().Subject;
        var wrapped = Model.NewModel.Step.Wrap(step, graph);

        Assert.AreEqual("POST", wrapped.Request.Method);
        Assert.HasCount(1, wrapped.Request.Headers);

        var extractor = (Model.NewModel.Extractors.HeaderExtractor)wrapped.Extract.Single();
        Assert.AreEqual("Location", extractor.HeaderName);
        Assert.AreEqual("jsonResourceLocation", extractor.To);

        var literals = GraphAssert.Reachable(graph, [step]).Triples.Select(t => t.Object).Where(o => o.NodeType == NodeType.Literal).Select(o => ((ILiteralNode)o).Value).ToArray();
        CollectionAssert.Contains(literals, "application/json");
        CollectionAssert.Contains(literals, "{\"a\":1}");
    }

    [TestMethod]
    public void ParseExpandsDeleteResourceInFromSpec()
    {
        var graph = Resources.Parse(new(Resources.Base, "fromSpec.ttl"));

        var step = graph.GetTriplesWithPredicateObject(graph.CreateUriNode(new Uri(Vocabulary + "stepName")), graph.CreateLiteralNode("Delete text resource")).Single().Subject;
        var wrapped = Model.NewModel.Step.Wrap(step, graph);

        Assert.AreEqual("DELETE", wrapped.Request.Method);
        Assert.IsEmpty(wrapped.Request.Headers);

        var extractor = (Model.NewModel.Extractors.StatusCodeExtractor)wrapped.Extract.Single();
        Assert.AreEqual("status", extractor.To);
    }

    [TestMethod]
    public void ParseExpandsDeleteResourceIfMatchInFromSpec()
    {
        var graph = Resources.Parse(new(Resources.Base, "fromSpec.ttl"));

        var step = graph.GetTriplesWithPredicateObject(graph.CreateUriNode(new Uri(Vocabulary + "stepName")), graph.CreateLiteralNode("Delete text resource with matching If-Match")).Single().Subject;
        var wrapped = Model.NewModel.Step.Wrap(step, graph);

        Assert.AreEqual("DELETE", wrapped.Request.Method);
        Assert.HasCount(1, wrapped.Request.Headers);

        var extractor = (Model.NewModel.Extractors.StatusCodeExtractor)wrapped.Extract.Single();
        Assert.AreEqual("status", extractor.To);

        var literals = GraphAssert.Reachable(graph, [step]).Triples.Select(t => t.Object).Where(o => o.NodeType == NodeType.Literal).Select(o => ((ILiteralNode)o).Value).ToArray();
        CollectionAssert.Contains(literals, "If-Match");
        CollectionAssert.Contains(literals, "etag");
    }

    [TestMethod]
    public void ParseExpandsPutResourceIfMatchInFromSpec()
    {
        var graph = Resources.Parse(new(Resources.Base, "fromSpec.ttl"));

        var step = graph.GetTriplesWithPredicateObject(graph.CreateUriNode(new Uri(Vocabulary + "stepName")), graph.CreateLiteralNode("Put JSON resource replacing content")).Single().Subject;
        var wrapped = Model.NewModel.Step.Wrap(step, graph);

        Assert.AreEqual("PUT", wrapped.Request.Method);
        Assert.HasCount(2, wrapped.Request.Headers);

        var extractor = (Model.NewModel.Extractors.StatusCodeExtractor)wrapped.Extract.Single();
        Assert.AreEqual("status", extractor.To);

        var literals = GraphAssert.Reachable(graph, [step]).Triples.Select(t => t.Object).Where(o => o.NodeType == NodeType.Literal).Select(o => ((ILiteralNode)o).Value).ToArray();
        CollectionAssert.Contains(literals, "If-Match");
        CollectionAssert.Contains(literals, "Content-Type");
        CollectionAssert.Contains(literals, "etag");
    }

    [TestMethod]
    public void ParseExpandsPatchResourceIfMatchInFromSpec()
    {
        var graph = Resources.Parse(new(Resources.Base, "fromSpec.ttl"));

        var step = graph.GetTriplesWithPredicateObject(graph.CreateUriNode(new Uri(Vocabulary + "stepName")), graph.CreateLiteralNode("Patch linkset adding describedby")).Single().Subject;
        var wrapped = Model.NewModel.Step.Wrap(step, graph);

        Assert.AreEqual("PATCH", wrapped.Request.Method);
        Assert.HasCount(2, wrapped.Request.Headers);

        var extractor = (Model.NewModel.Extractors.StatusCodeExtractor)wrapped.Extract.Single();
        Assert.AreEqual("status", extractor.To);

        var literals = GraphAssert.Reachable(graph, [step]).Triples.Select(t => t.Object).Where(o => o.NodeType == NodeType.Literal).Select(o => ((ILiteralNode)o).Value).ToArray();
        CollectionAssert.Contains(literals, "If-Match");
        CollectionAssert.Contains(literals, "Content-Type");
        CollectionAssert.Contains(literals, "etag");
    }

    [TestMethod]
    public void ParseExpandsReadContainerInFromSpec()
    {
        var graph = Resources.Parse(new(Resources.Base, "fromSpec.ttl"));

        var step = graph.GetTriplesWithPredicateObject(graph.CreateUriNode(new Uri(Vocabulary + "stepName")), graph.CreateLiteralNode("Get container id")).Single().Subject;
        var wrapped = Model.NewModel.Step.Wrap(step, graph);

        Assert.AreEqual("GET", wrapped.Request.Method);
        Assert.HasCount(1, wrapped.Request.Headers);

        var extractor = (Model.NewModel.Extractors.JsonPathExtractor)wrapped.Extract.Single();
        Assert.AreEqual("$.id", extractor.Path);
        Assert.AreEqual("containerId", extractor.To);
    }

    [TestMethod]
    public void ParseExpandsTemplatesInTouchstone()
    {
        var graph = Resources.Parse(new(Resources.Base, "touchstone.ttl"));

        var leftovers = graph.Triples
            .Where(t => t.Predicate.ToString() is Vocabulary + "argument" or Vocabulary + "expansion"
                || t.Predicate.ToString() == RdfType && t.Object.ToString() is Vocabulary + "CreateContainer" or Vocabulary + "CreateResource" or Vocabulary + "DeleteResource" or Vocabulary + "DeleteResourceIfMatch" or Vocabulary + "PutResourceIfMatch" or Vocabulary + "PatchResourceIfMatch" or Vocabulary + "ReadContainer" or Vocabulary + "StepTemplate")
            .ToArray();
        Assert.IsEmpty(leftovers);

        var step = graph.GetTriplesWithPredicateObject(graph.CreateUriNode(new Uri(Vocabulary + "stepName")), graph.CreateLiteralNode("Create digits resource")).Single().Subject;
        var wrapped = Model.NewModel.Step.Wrap(step, graph);

        Assert.AreEqual("POST", wrapped.Request.Method);
        Assert.HasCount(1, wrapped.Request.Headers);

        var extractor = (Model.NewModel.Extractors.HeaderExtractor)wrapped.Extract.Single();
        Assert.AreEqual("Location", extractor.HeaderName);
        Assert.AreEqual("digitsResourceLocation", extractor.To);

        var literals = GraphAssert.Reachable(graph, [step]).Triples.Select(t => t.Object).Where(o => o.NodeType == NodeType.Literal).Select(o => ((ILiteralNode)o).Value).ToArray();
        CollectionAssert.Contains(literals, "text/plain");
        CollectionAssert.Contains(literals, "0123456789");
    }

    [TestMethod]
    public void ParseExpandsTemplatesInOldManifest()
    {
        var graph = Resources.Parse(new(Resources.Base, "oldmanifest.ttl"));

        var leftovers = graph.Triples
            .Where(t => t.Predicate.ToString() is Vocabulary + "argument" or Vocabulary + "expansion"
                || t.Predicate.ToString() == RdfType && t.Object.ToString() is Vocabulary + "CreateContainer" or Vocabulary + "CreateResource" or Vocabulary + "DeleteResource" or Vocabulary + "DeleteResourceIfMatch" or Vocabulary + "PutResourceIfMatch" or Vocabulary + "PatchResourceIfMatch" or Vocabulary + "ReadContainer" or Vocabulary + "StepTemplate")
            .ToArray();
        Assert.IsEmpty(leftovers);

        var step = graph.GetTriplesWithPredicateObject(graph.CreateUriNode(new Uri(Vocabulary + "stepName")), graph.CreateLiteralNode("Create shopping list")).Single().Subject;
        var wrapped = Model.NewModel.Step.Wrap(step, graph);

        Assert.AreEqual("POST", wrapped.Request.Method);
        Assert.HasCount(1, wrapped.Request.Headers);
    }

    [TestMethod]
    public void ParseExpandsTemplatesInExample()
    {
        var graph = Resources.Parse(new(Resources.Base, "example.ttl"));

        var leftovers = graph.Triples
            .Where(t => t.Predicate.ToString() is Vocabulary + "argument" or Vocabulary + "expansion"
                || t.Predicate.ToString() == RdfType && t.Object.ToString() is Vocabulary + "CreateContainer" or Vocabulary + "CreateResource" or Vocabulary + "DeleteResource" or Vocabulary + "DeleteResourceIfMatch" or Vocabulary + "PutResourceIfMatch" or Vocabulary + "PatchResourceIfMatch" or Vocabulary + "ReadContainer" or Vocabulary + "StepTemplate")
            .ToArray();
        Assert.IsEmpty(leftovers);

        var step = graph.GetTriplesWithPredicateObject(graph.CreateUriNode(new Uri(Vocabulary + "stepName")), graph.CreateLiteralNode("Delete resource")).Single().Subject;
        var wrapped = Model.NewModel.Step.Wrap(step, graph);

        Assert.AreEqual("DELETE", wrapped.Request.Method);
    }

    private static IEnumerable<INode> ManifestNodes(IGraph graph) =>
        graph.GetTriplesWithPredicate(graph.CreateUriNode(new Uri(Vocabulary + "manifestName"))).Select(t => t.Subject).ToArray();
}
