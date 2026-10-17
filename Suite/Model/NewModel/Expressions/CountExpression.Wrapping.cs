namespace Model.NewModel.Expressions;

public partial class CountExpression : Expression
{
    protected CountExpression(INode node, IGraph graph) : base(node, graph) { }

    public Expression Value => this.Singular(Vocabulary.Count, Expression.Choose);

    public static CountExpression Wrap(INode node, IGraph graph) => new(node, graph);

    public static CountExpression? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
