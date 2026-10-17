namespace Model.NewModel.Assertions;

public partial class RegexAssertion
{
    protected override Result ExecuteInternal(object input)
    {
        var actual = input as string ?? throw new InvalidOperationException("Input is not a string.");
        return Pattern.IsMatch(actual)
            ? new Result { Outcome = "Pass", Info = $"Input matches '{Pattern}'." }
            : new Result { Outcome = "Fail", Info = $"Input expected to match '{Pattern}', but got '{actual}'." };
    }
}
