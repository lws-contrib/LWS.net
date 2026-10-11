namespace Model.NewModel.Expressions;

public partial class ParamRefExpression
{
    public override async Task<string> Evaluate(Context context)
    {
        var value = context.Get(Param) as string ?? throw new InvalidOperationException($"Parameter '{Param}' is not a string.");

        context.Log<ParamRefExpression>("Evaluate: [{Param}] = [{Value}]", Param, value);

        return value;
    }
}
