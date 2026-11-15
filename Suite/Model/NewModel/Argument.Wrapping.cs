namespace Model.NewModel;

public class Argument : GraphWrapperNode
{
    protected Argument(INode node, IGraph graph) : base(node, graph) { }

    public string? ParameterName
    {
        get => this.Singular(Vocabulary.ParameterName, ValueMappings.As<string>);

        set => this.OverwriteNullable(Vocabulary.ParameterName, value);
    }

    public INode? Value
    {
        get => this.Singular(Vocabulary.Value, ValueMappings.AsIs);

        set => this.OverwriteNullable(Vocabulary.Value, value);
    }

    public static Argument Wrap(INode node, IGraph graph) => new(node, graph);

    public static Argument? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
