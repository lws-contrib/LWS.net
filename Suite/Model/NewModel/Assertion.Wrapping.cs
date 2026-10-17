using Model.NewModel.Assertions;

namespace Model.NewModel;

public abstract partial class Assertion : GraphWrapperNode
{
    protected Assertion(INode node, IGraph graph) : base(node, graph) { }

    public Expression Input => this.Singular(Vocabulary.Input, Expression.Choose);

    internal static Assertion? Choose(GraphWrapperNode node)
    {
        if (node.Graph.GetTriplesWithSubjectPredicate(node, Vocabulary.Value).Any())
        {
            return EqualityAssertion.Wrap(node);
        }

        if (node.Graph.GetTriplesWithSubjectPredicate(node, Vocabulary.Regex).Any())
        {
            return RegexAssertion.Wrap(node);
        }

        if (node.Graph.GetTriplesWithSubjectPredicate(node, Vocabulary.Contains).Any())
        {
            return ContainsAssertion.Wrap(node);
        }

        throw new InvalidOperationException();
    }
}
