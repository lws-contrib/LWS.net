namespace Model.NewModel.Extractors;

public partial class HeaderExtractor
{
    protected override async Task<object> Extract(Context context, HttpResponseMessage response)
    {
        var values = response.Headers.TryGetValues(HeaderName, out var responseValues)
            ? responseValues
            : response.Content.Headers.GetValues(HeaderName);

        var value = values.ToArray();

        context.Log<HeaderExtractor>("Extract: [{HeaderName}] = [{Value}]", HeaderName, value);

        return value;
    }
}
