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

    public static void IsomorphicFrom(string expectedTurtle, IGraph actual, params Uri[] roots)
    {
        var expected = new Graph();
        new TurtleParser().Load(expected, new System.IO.StringReader(expectedTurtle));

        var expectedSubgraph = Reachable(expected, roots);
        var actualSubgraph = Reachable(actual, roots);

        if (expectedSubgraph.Equals(actualSubgraph))
        {
            return;
        }

        Assert.Fail($"Subgraphs reachable from {string.Join(", ", roots.Select(r => r.AbsoluteUri))} are not isomorphic.{Environment.NewLine}Expected:{Environment.NewLine}{Serialize(expectedSubgraph)}{Environment.NewLine}Actual:{Environment.NewLine}{Serialize(actualSubgraph)}");
    }

    public static IGraph Reachable(IGraph graph, params Uri[] roots) =>
        Reachable(graph, roots.Select(root => (INode)graph.CreateUriNode(root)));

    public static IGraph Reachable(IGraph graph, IEnumerable<INode> roots)
    {
        var result = new Graph();
        var pending = new Stack<INode>(roots);
        var seen = pending.ToHashSet();

        while (pending.TryPop(out var node))
        {
            foreach (var triple in graph.GetTriplesWithSubject(node).ToArray())
            {
                result.Assert(triple);

                if (triple.Object.NodeType == NodeType.Blank && seen.Add(triple.Object))
                {
                    pending.Push(triple.Object);
                }
            }
        }

        return result;
    }

    private static string Serialize(IGraph graph)
    {
        using var writer = new System.IO.StringWriter();
        new CompressingTurtleWriter().Save(graph, writer);
        return writer.ToString();
    }
}
