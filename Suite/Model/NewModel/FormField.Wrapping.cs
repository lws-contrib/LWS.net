namespace Model.NewModel;

public class FormField : GraphWrapperNode
{
    protected FormField(INode node, IGraph graph) : base(node, graph) { }

    public string Key => this.Singular(Vocabulary.FormKey, ValueMappings.As<string>);

    public Expression Value => this.Singular(Vocabulary.FormValue, Expression.Choose);

    public static FormField Wrap(INode node, IGraph graph) => new(node, graph);

    public static FormField? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
