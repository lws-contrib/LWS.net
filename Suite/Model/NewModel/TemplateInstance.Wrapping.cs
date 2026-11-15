namespace Model.NewModel;

public class TemplateInstance : Typed
{
    protected TemplateInstance(INode node, IGraph graph) : base(node, graph) { }

    public ISet<Argument> Arguments => this.Objects(Vocabulary.Argument, Argument.Wrap, Argument.Wrap);

    public static TemplateInstance Wrap(INode node, IGraph graph) => new(node, graph);

    public static TemplateInstance? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
