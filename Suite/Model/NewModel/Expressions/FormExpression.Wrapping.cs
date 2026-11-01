namespace Model.NewModel.Expressions;

public partial class FormExpression : Expression
{
    protected FormExpression(INode node, IGraph graph) : base(node, graph) { }

    public IList<FormField> Fields => this.List(Vocabulary.Form, FormField.Wrap, FormField.Wrap);

    public static FormExpression Wrap(INode node, IGraph graph) => new(node, graph);

    public static FormExpression? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
