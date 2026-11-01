using Model.NewModel.Expressions;

namespace Model.NewModel;

public abstract partial class Expression : GraphWrapperNode
{
    protected Expression(INode node, IGraph graph) : base(node, graph) { }

    internal static Expression? Choose(GraphWrapperNode? node)
    {
        if (node is null)
        {
            return null;
        }

        if (node.Graph.GetTriplesWithSubjectPredicate(node, Vocabulary.Constant).Any())
        {
            return ConstantExpression.Wrap(node);
        }

        if (node.Graph.GetTriplesWithSubjectPredicate(node, Vocabulary.Variable).Any())
        {
            return VariableExpression.Wrap(node);
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

        if (node.Graph.GetTriplesWithSubjectPredicate(node, Vocabulary.Form).Any())
        {
            return FormExpression.Wrap(node);
        }

        throw new InvalidOperationException();
    }
}
