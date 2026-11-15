using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;

namespace ModelTest;

public static class GraphAssert
{
    public static void Isomorphic(string expectedTurtle, IGraph actual)
    {
        var expected = new Graph();
        new TurtleParser().Load(expected, new System.IO.StringReader(expectedTurtle));

        if (expected.Equals(actual))
        {
            return;
        }

        Assert.Fail($"Graphs are not isomorphic.{Environment.NewLine}Expected:{Environment.NewLine}{Serialize(expected)}{Environment.NewLine}Actual:{Environment.NewLine}{Serialize(actual)}");
    }

    private static string Serialize(IGraph graph)
    {
        using var writer = new System.IO.StringWriter();
        new CompressingTurtleWriter().Save(graph, writer);
        return writer.ToString();
    }
}
