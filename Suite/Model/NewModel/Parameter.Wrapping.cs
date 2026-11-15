namespace Model.NewModel;

public class Parameter : GraphWrapperNode
{
    protected Parameter(INode node, IGraph graph) : base(node, graph) { }

    public string? ParameterName
    {
        get => this.Singular(Vocabulary.ParameterName, ValueMappings.As<string>);

        set => this.OverwriteNullable(Vocabulary.ParameterName, value);
    }

    public INode? Default
    {
        get => this.Singular(Vocabulary.Default, ValueMappings.AsIs);

        set => this.OverwriteNullable(Vocabulary.Default, value);
    }

    public bool? Required
    {
        get => this.Singular(Vocabulary.Required, ValueMappings.As<bool?>);

        set => this.OverwriteNullable(Vocabulary.Required, value);
    }

    public static Parameter Wrap(INode node, IGraph graph) => new(node, graph);

    public static Parameter? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
