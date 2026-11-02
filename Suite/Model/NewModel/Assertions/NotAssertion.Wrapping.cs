namespace Model.NewModel.Assertions;

public partial class NotAssertion : Assertion
{
    protected NotAssertion(INode node, IGraph graph) : base(node, graph) { }

    public Assertion Inner => this.Singular(Vocabulary.Not, Assertion.Choose);

    public static NotAssertion Wrap(INode node, IGraph graph) => new(node, graph);

    public static NotAssertion? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
