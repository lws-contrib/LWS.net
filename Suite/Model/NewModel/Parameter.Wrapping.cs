namespace Model.NewModel;

public class Parameter : GraphWrapperNode
{
    protected Parameter(INode node, IGraph graph) : base(node, graph) { }

    public string? ParameterName
    => this.Singular(Vocabulary.ParameterName, ValueMappings.As<string>);

    public INode? Default
    => this.Singular(Vocabulary.Default, ValueMappings.AsIs);

    public bool? Required
    => this.Singular(Vocabulary.Required, ValueMappings.As<bool?>);

    public static Parameter Wrap(INode node, IGraph graph) => new(node, graph);

    public static Parameter? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
