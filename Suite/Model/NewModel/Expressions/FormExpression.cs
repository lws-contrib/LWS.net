namespace Model.NewModel.Expressions;

public partial class FormExpression
{
    public override async Task<object> Evaluate(Context context)
    {
        var form = new List<KeyValuePair<string, string>>();

        foreach (var field in Fields)
        {
            var value = await field.Value.Evaluate(context) as string ?? throw new InvalidOperationException($"Value of form field [{field.Key}] is not a string.");

            form.Add(new(field.Key, value));
        }

        context.Log<FormExpression>("Evaluate: [{Count}] fields", form.Count);

        return form;
    }
}
