namespace Model.NewModel.Assertions;

public partial class EqualityAssertion
{
    protected override async Task<Result> ExecuteInternal(object input, Context context)
    {
        var expected = await EqualsValue.Evaluate(context);
        return Equals(input, expected)
            ? new Result { Outcome = "Pass", Info = $"Input equals '{expected}'." }
            : new Result { Outcome = "Fail", Info = $"Input expected '{expected}', but got '{input}'." };
    }
}
