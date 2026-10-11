namespace Model.NewModel.Extractors;

public partial class LinkExtractor : Extractor
{
    protected LinkExtractor(INode node, IGraph graph) : base(node, graph) { }

    public string Rel => this.Singular(Vocabulary.Link, ValueMappings.As<string>);

    public static LinkExtractor Wrap(INode node, IGraph graph) => new(node, graph);

    public static LinkExtractor? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
