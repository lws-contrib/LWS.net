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

        var actual = Resources.Parse(NewUri);

        Assert.IsTrue(plain.Equals(actual));
    }

    [TestMethod]
    public void ParseExpandsCreateContainerInFromSpec()
    {
        var graph = Resources.Parse(new(Resources.Base, "fromSpec.ttl"));

        var leftovers = graph.Triples
            .Where(t => t.Predicate.ToString() is Vocabulary + "argument" or Vocabulary + "parameterName" or Vocabulary + "param"
                || t.Predicate.ToString() == RdfType && t.Object.ToString() is Vocabulary + "CreateContainer" or Vocabulary + "StepTemplate")
            .ToArray();
        Assert.IsEmpty(leftovers);

        var step = graph.GetTriplesWithPredicateObject(graph.CreateUriNode(new Uri(Vocabulary + "stepName")), graph.CreateLiteralNode("Create container")).Single().Subject;
        var request = Model.NewModel.Step.Wrap(step, graph).Request;

        Assert.AreEqual("POST", request.Method);
        Assert.HasCount(1, request.Headers);
    }
}
