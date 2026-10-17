namespace Model.NewModel.Assertions;

public partial class GreaterThanAssertion
{
    protected override Result ExecuteInternal(object input)
    {
        var actual = input as long? ?? throw new InvalidOperationException("Input is not an integer.");
        return actual > Value
            ? new Result { Outcome = "Pass", Info = $"Input is greater than {Value}." }
            : new Result { Outcome = "Fail", Info = $"Input expected to be greater than {Value}, but got {actual}." };
    }
}
