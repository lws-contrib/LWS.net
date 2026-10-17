namespace Model.NewModel.Expressions;

public partial class ConstantExpression
{
    public override async Task<object> Evaluate(Context context)
    {
        context.Log<ConstantExpression>("Evaluate [{Constant}]", Constant);

        return Constant;
    }
}
