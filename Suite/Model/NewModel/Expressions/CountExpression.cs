namespace Model.NewModel.Expressions;

public partial class CountExpression
{
    public override async Task<object> Evaluate(Context context)
    {
        var values = await Value.Evaluate(context) as string[] ?? throw new InvalidOperationException("Value is not an array of strings.");
        var count = (long)values.Length;

        context.Log<CountExpression>("Evaluate: [{Values}] = [{Count}]", values, count);

        return count;
    }
}
