namespace Model.NewModel.Expressions;

public partial class VariableExpression
{
    public override async Task<object> Evaluate(Context context)
    {
        var value = context.Get(Variable);

        context.Log<VariableExpression>("Evaluate: [{Variable}] = [{Value}]", Variable, value);

        return value;
    }
}
