using Model.NewModel.Expressions;

namespace Model.NewModel;

public abstract partial class Expression : GraphWrapperNode
{
    protected Expression(INode node, IGraph graph) : base(node, graph) { }

    internal static Expression? Choose(GraphWrapperNode node)
    {
        if (node.Graph.GetTriplesWithSubjectPredicate(node, Vocabulary.Value).Any())
        {
            return ConstantExpression.Wrap(node);
        }

        if (node.Graph.GetTriplesWithSubjectPredicate(node, Vocabulary.Variable).Any())
        {
            return ParamRefExpression.Wrap(node);
        }

        if (node.Graph.GetTriplesWithSubjectPredicate(node, Vocabulary.Base).Any())
        {
            return ResolveExpression.Wrap(node);
        }

        if (node.Graph.GetTriplesWithSubjectPredicate(node, Vocabulary.Single).Any())
        {
            return SingleExpression.Wrap(node);
        }

        if (node.Graph.GetTriplesWithSubjectPredicate(node, Vocabulary.Count).Any())
        {
            return CountExpression.Wrap(node);
        }

        throw new InvalidOperationException();
    }
}
