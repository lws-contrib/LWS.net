namespace Model.NewModel;

public class Argument : GraphWrapperNode
{
    protected Argument(INode node, IGraph graph) : base(node, graph) { }

    public string? ParameterName
    => this.Singular(Vocabulary.ParameterName, ValueMappings.As<string>);

    public INode? Value
    => this.Singular(Vocabulary.Value, ValueMappings.AsIs);

    public static Argument Wrap(INode node, IGraph graph) => new(node, graph);

    public static Argument? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
