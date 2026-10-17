namespace Model.NewModel.Assertions;

public partial class GreaterThanAssertion : Assertion
{
    protected GreaterThanAssertion(INode node, IGraph graph) : base(node, graph) { }

    public long Value => this.Singular(Vocabulary.GreaterThan, ValueMappings.As<long>);

    public static GreaterThanAssertion Wrap(INode node, IGraph graph) => new(node, graph);

    public static GreaterThanAssertion? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
