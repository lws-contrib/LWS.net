namespace Model.NewModel.Assertions;

public partial class ContainsAssertion
{
    protected internal override async Task<Result> ExecuteInternal(object input, Context context)
    {
        var actual = input as string[] ?? throw new InvalidOperationException("Input is not an array of strings.");
        var expected = await Value.Evaluate(context) as string ?? throw new InvalidOperationException("Value of contains is not a string.");
        return actual.Contains(expected)
            ? new Result { Outcome = "Pass", Info = $"Input contains '{expected}'." }
            : new Result { Outcome = "Fail", Info = $"Input expected to contain '{expected}', but got [{string.Join(", ", actual)}]." };
    }
}
