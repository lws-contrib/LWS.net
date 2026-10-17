using System.Text.RegularExpressions;

namespace Model.NewModel.Assertions;

public partial class RegexAssertion : Assertion
{
    protected RegexAssertion(INode node, IGraph graph) : base(node, graph) { }

    public Regex Matches => this.Singular(Vocabulary.Matches, ValueMappings.RegexFromStringLiteral);

    public static RegexAssertion Wrap(INode node, IGraph graph) => new(node, graph);

    public static RegexAssertion? Wrap(GraphWrapperNode node) => node switch { null => default, _ => Wrap(node, node.Graph) };
}
