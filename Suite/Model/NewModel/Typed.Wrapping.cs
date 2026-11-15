namespace Model.NewModel;

public abstract class Typed(INode node, IGraph graph) : GraphWrapperNode(node, graph)
{
    public ISet<Uri> Type => this.Objects(Vocabulary.RdfType, NodeMappings.From, ValueMappings.As<Uri>);
}
