namespace Model.NewModel.Expressions;

public partial class SingleExpression
{
    public override async Task<object> Evaluate(Context context)
    {
        var values = await Value.Evaluate(context) as string[] ?? throw new InvalidOperationException("Value is not an array of strings.");
        var value = values.Single();

        context.Log<SingleExpression>("Evaluate: [{Values}] = [{Value}]", values, value);

        return value;
    }
}
