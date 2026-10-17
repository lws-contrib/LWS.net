namespace Model.NewModel;

public class Manifest : GraphWrapperNode
{
    protected Manifest(INode node, IGraph graph) : base(node, graph) { }

    public string ManifestName => this.Singular(Vocabulary.ManifestName, ValueMappings.As<string>);

    public ISet<TestCase> Test => this.Objects(Vocabulary.Test, TestCase.Wrap, TestCase.Wrap);

    public ISet<Uri> Include => this.Objects(Vocabulary.Include, NodeMappings.From, ValueMappings.As<Uri>);

    public static Manifest Wrap(INode node, IGraph graph) => new(node, graph);

    public static Manifest? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
