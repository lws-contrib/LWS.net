namespace Model.NewModel;

public abstract partial class Expression
{
    public abstract Task<object> Evaluate(Context context);
}
