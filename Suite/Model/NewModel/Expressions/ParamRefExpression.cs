namespace Model.NewModel.Expressions;

public partial class ParamRefExpression
{
    public override async Task<object> Evaluate(Context context)
    {
        var value = context.Get(Variable);

        context.Log<ParamRefExpression>("Evaluate: [{Variable}] = [{Value}]", Variable, value);

        return value;
    }
}
