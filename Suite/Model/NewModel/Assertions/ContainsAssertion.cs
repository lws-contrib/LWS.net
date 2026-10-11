namespace Model.NewModel.Assertions;

public partial class ContainsAssertion
{
    public override Result Execute(Context context)
    {
        var actual = context.Get(Param) as string[] ?? throw new InvalidOperationException($"Parameter '{Param}' is not an array of strings.");
        return actual.Contains(Value)
            ? new Result { Outcome = "Pass", Info = $"Parameter '{Param}' contains '{Value}'." }
            : new Result { Outcome = "Fail", Info = $"Parameter '{Param}' expected to contain '{Value}', but got [{string.Join(", ", actual)}]." };
    }
}
