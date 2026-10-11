namespace Model.NewModel.Assertions;

public partial class EqualityAssertion
{
    public override Result Execute(Context context)
    {
        var actual = context.Get(Param) as string ?? throw new InvalidOperationException($"Parameter '{Param}' is not a string.");
        return actual == Value
            ? new Result { Outcome = "Pass", Info = $"Parameter '{Param}' equals '{Value}'." }
            : new Result { Outcome = "Fail", Info = $"Parameter '{Param}' expected '{Value}', but got '{actual}'." };
    }
}
