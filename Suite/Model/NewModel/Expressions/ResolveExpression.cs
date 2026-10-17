namespace Model.NewModel.Expressions;

public partial class ResolveExpression
{
    public override async Task<object> Evaluate(Context context)
    {
        var baseUri = await Base.Evaluate(context) as string ?? throw new InvalidOperationException("Base is not a string.");
        var uri = await Relative.Evaluate(context) as string ?? throw new InvalidOperationException("Relative is not a string.");
        var result = new Uri(new Uri(baseUri), uri);

        context.Log<ResolveExpression>("Evaluate: baseUri = [{BaseUri}], relativeUri = [{RelativeUri}], result = [{Result}]", baseUri, uri, result);

        return result.ToString();
    }
}
