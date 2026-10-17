namespace Model.NewModel;

public abstract partial class Assertion
{
    public async Task<Result> Execute(Context context) => ExecuteInternal(await That.Evaluate(context));

    protected abstract Result ExecuteInternal(object input);
}
