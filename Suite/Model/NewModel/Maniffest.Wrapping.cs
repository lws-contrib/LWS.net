namespace Model.NewModel;

public class Manifest : GraphWrapperNode
{
    protected Manifest(INode node, IGraph graph) : base(node, graph) { }

    public string Name => this.Singular(Vocabulary.Name, ValueMappings.As<string>);

    public ISet<TestCase> Tests => this.Objects(Vocabulary.Tests, TestCase.Wrap, TestCase.Wrap);

    public ISet<Uri> Include => this.Objects(Vocabulary.Include, NodeMappings.From, ValueMappings.As<Uri>);

    public static Manifest Wrap(INode node, IGraph graph) => new(node, graph);

    public static Manifest? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
