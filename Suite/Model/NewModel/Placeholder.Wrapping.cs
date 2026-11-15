namespace Model.NewModel;

public class Placeholder : GraphWrapperNode
{
    protected Placeholder(INode node, IGraph graph) : base(node, graph) { }

    public string? ParameterName
    {
        get => this.Singular(Vocabulary.Param, ValueMappings.As<string>);

        set => this.OverwriteNullable(Vocabulary.Param, value);
    }

    public static Placeholder Wrap(INode node, IGraph graph) => new(node, graph);
}
