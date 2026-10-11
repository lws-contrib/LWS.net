namespace Model.NewModel.Assertions;

public partial class ContainsAssertion : Assertion
{
    protected ContainsAssertion(INode node, IGraph graph) : base(node, graph) { }

    public string Value => this.Singular(Vocabulary.Contains, ValueMappings.As<string>);

    public static ContainsAssertion Wrap(INode node, IGraph graph) => new(node, graph);

    public static ContainsAssertion? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
