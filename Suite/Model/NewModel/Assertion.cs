namespace Model.NewModel;

public abstract partial class Assertion
{
    public async Task<Result> Execute(Context context) => await ExecuteInternal(await That.Evaluate(context), context);

    protected internal abstract Task<Result> ExecuteInternal(object input, Context context);
}
