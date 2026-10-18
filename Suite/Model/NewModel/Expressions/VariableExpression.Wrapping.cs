namespace Model.NewModel.Expressions;

public partial class VariableExpression : Expression
{
    protected VariableExpression(INode node, IGraph graph) : base(node, graph) { }

    public string Variable => this.Singular(Vocabulary.Variable, ValueMappings.As<string>);

    public static VariableExpression Wrap(INode node, IGraph graph) => new(node, graph);

    public static VariableExpression? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
