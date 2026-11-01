namespace Model.NewModel.Assertions;

public partial class EqualityAssertion
{
    protected override async Task<Result> ExecuteInternal(object input, Context context)
    {
        var actual = input as string ?? throw new InvalidOperationException("Input is not a string.");
        var expected = await EqualsValue.Evaluate(context) as string ?? throw new InvalidOperationException("Value of equals is not a string.");
        return actual == expected
            ? new Result { Outcome = "Pass", Info = $"Input equals '{expected}'." }
            : new Result { Outcome = "Fail", Info = $"Input expected '{expected}', but got '{actual}'." };
    }
}
