namespace Model.NewModel.Expressions;

public partial class SingleExpression : Expression
{
    protected SingleExpression(INode node, IGraph graph) : base(node, graph) { }

    public Expression Value => this.Singular(Vocabulary.Single, Expression.Choose);

    public static SingleExpression Wrap(INode node, IGraph graph) => new(node, graph);

    public static SingleExpression? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
