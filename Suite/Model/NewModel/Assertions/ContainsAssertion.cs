namespace Model.NewModel.Assertions;

public partial class ContainsAssertion
{
    protected override Result ExecuteInternal(object input)
    {
        var actual = input as string[] ?? throw new InvalidOperationException("Input is not an array of strings.");
        return actual.Contains(Value)
            ? new Result { Outcome = "Pass", Info = $"Input contains '{Value}'." }
            : new Result { Outcome = "Fail", Info = $"Input expected to contain '{Value}', but got [{string.Join(", ", actual)}]." };
    }
}
