using VDS.RDF;
using VDS.RDF.Parsing;

namespace ModelTest;

[TestClass]
public class GraphAssertTest
{
    [TestMethod]
    public void IgnoresBlankNodeLabels()
    {
        var actual = Parse("@prefix : <http://example.org/> . :s :p _:b . _:b :q 1 .");

        GraphAssert.Isomorphic("@prefix : <http://example.org/> . :s :p _:x . _:x :q 1 .", actual);
    }

    [TestMethod]
    public void FailsOnDifferentGraphs()
    {
        var actual = Parse("@prefix : <http://example.org/> . :s :p 1 .");

        Assert.ThrowsExactly<AssertFailedException>(() => GraphAssert.Isomorphic("@prefix : <http://example.org/> . :s :p 2 .", actual));
    }

    private static Graph Parse(string turtle)
    {
        var graph = new Graph();
        new TurtleParser().Load(graph, new System.IO.StringReader(turtle));
        return graph;
    }
}
