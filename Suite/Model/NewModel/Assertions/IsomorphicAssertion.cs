namespace Model.NewModel.Assertions;

public partial class IsomorphicAssertion
{
    protected internal override async Task<Result> ExecuteInternal(object input, Context context)
    {
        var actual = input as IGraph ?? throw new InvalidOperationException("Input is not a graph.");
        var expected = await IsomorphicWith.Evaluate(context) as IGraph ?? throw new InvalidOperationException("Value of isomorphic with is not a graph.");

        return actual.Equals(expected, out _)
            ? new Result { Outcome = "Pass", Info = $"Input is isomorphic with the expected graph ({actual.Triples.Count} triples)." }
            : new Result { Outcome = "Fail", Info = $"Input is not isomorphic with the expected graph: {actual.Triples.Count} triples in input, {expected.Triples.Count} in expected." };
    }
}
