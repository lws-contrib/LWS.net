namespace Model.NewModel.Assertions;

public partial class EqualityAssertion
{
    protected override Result ExecuteInternal(object input)
    {
        var actual = input as string ?? throw new InvalidOperationException("Input is not a string.");
        return actual == Value
            ? new Result { Outcome = "Pass", Info = $"Input equals '{Value}'." }
            : new Result { Outcome = "Fail", Info = $"Input expected '{Value}', but got '{actual}'." };
    }
}
