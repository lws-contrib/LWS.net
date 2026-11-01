using System.Net.Http.Headers;

namespace Model.NewModel;

public partial class Request
{
    public async Task<HttpResponseMessage> Execute(Context context)
    {
        var u = await Uri.Evaluate(context) as string ?? throw new InvalidOperationException("Uri is not a string.");
        var re = new HttpRequestMessage(new HttpMethod(Method), u);

        if (Body is { } body)
        {
            re.Content = await body.Evaluate(context) switch
            {
                string content => new StringContent(content, null as MediaTypeHeaderValue),
                IEnumerable<KeyValuePair<string, string>> fields => new FormUrlEncodedContent(fields),
                _ => throw new InvalidOperationException("Body is not a string or form fields.")
            };
        }

        foreach (var header in Headers)
        {
            var value = await header.Value.Evaluate(context) as string ?? throw new InvalidOperationException($"Value of header [{header.Key}] is not a string.");

            if (!re.Headers.TryAddWithoutValidation(header.Key, value))
            {
                re.Content?.Headers.TryAddWithoutValidation(header.Key, value);
            }
        }

        context.Log<Request>("Execute: [{Method}] [{Uri}]", Method, u);

        return await context.Client.SendAsync(re);
    }
}
