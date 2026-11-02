namespace Model.NewModel.Extractors;

public partial class GraphExtractor : Extractor
{
    protected GraphExtractor(INode node, IGraph graph) : base(node, graph) { }

    public static GraphExtractor Wrap(INode node, IGraph graph) => new(node, graph);

    public static GraphExtractor? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
