namespace Model.NewModel;

public class StepTemplate : Typed
{
    protected StepTemplate(INode node, IGraph graph) : base(node, graph) { }

    public IList<Parameter> Parameters => this.List(Vocabulary.Parameter, Parameter.Wrap, Parameter.Wrap);

    public INode? Expansion
    {
        get => this.Singular(Vocabulary.Expansion, ValueMappings.AsIs);

        set => this.OverwriteNullable(Vocabulary.Expansion, value);
    }

    public IEnumerable<TemplateInstance> Instances => this.Graph.InstancesOf(this, TemplateInstance.Wrap);

    public static StepTemplate Wrap(INode node, IGraph graph) => new(node, graph);

    public static StepTemplate? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
