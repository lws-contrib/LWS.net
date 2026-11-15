namespace Model.NewModel;

public class Placeholder : GraphWrapperNode
{
    protected Placeholder(INode node, IGraph graph) : base(node, graph) { }

    public string? ParameterName
    => this.Singular(Vocabulary.Param, ValueMappings.As<string>);

    public static Placeholder Wrap(INode node, IGraph graph) => new(node, graph);
}
