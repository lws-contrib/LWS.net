namespace Model.NewModel.Assertions;

public partial class NotAssertion
{
    protected internal override async Task<Result> ExecuteInternal(object input, Context context)
    {
        var inner = await Inner.ExecuteInternal(input, context);
        return inner.Outcome == "Pass"
            ? new Result { Outcome = "Fail", Info = $"Operand passed, but expected it to fail: {inner.Info}" }
            : new Result { Outcome = "Pass", Info = $"Operand failed as expected: {inner.Info}" };
    }
}
