namespace Model.NewModel.Assertions;

public partial class GreaterThanAssertion
{
    protected override async Task<Result> ExecuteInternal(object input, Context context)
    {
        var actual = input as long? ?? throw new InvalidOperationException("Input is not an integer.");
        var expected = await Value.Evaluate(context) as long? ?? throw new InvalidOperationException("Value of greater than is not an integer.");
        return actual > expected
            ? new Result { Outcome = "Pass", Info = $"Input is greater than {expected}." }
            : new Result { Outcome = "Fail", Info = $"Input expected to be greater than {expected}, but got {actual}." };
    }
}
