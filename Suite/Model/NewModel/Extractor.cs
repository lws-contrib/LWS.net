namespace Model.NewModel;

public abstract partial class Extractor
{
    public async Task Execute(Context context, HttpResponseMessage response)
    {
        context.Set(To, await Extract(context, response));
    }

    protected abstract Task<object> Extract(Context context, HttpResponseMessage response);
}
