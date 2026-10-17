namespace Model.NewModel.Expressions;

public partial class ConstantExpression : Expression
{
    protected ConstantExpression(INode node, IGraph graph) : base(node, graph) { }

    public string Constant => this.Singular(Vocabulary.Constant, ValueMappings.As<string>);

    public static ConstantExpression Wrap(INode node, IGraph graph) => new(node, graph);

    public static ConstantExpression? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
