namespace Model.NewModel;

public class RequestHeader : GraphWrapperNode
{
    protected RequestHeader(INode node, IGraph graph) : base(node, graph) { }

    public string Key => this.Singular(Vocabulary.Key, ValueMappings.As<string>);

    public Expression Value => this.Singular(Vocabulary.Value, Expression.Choose);

    public static RequestHeader Wrap(INode node, IGraph graph) => new(node, graph);

    public static RequestHeader? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
