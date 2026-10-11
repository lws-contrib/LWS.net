using Meziantou.Framework.Http;

namespace Model.NewModel.Extractors;

public partial class LinkExtractor
{
    protected override async Task<object> Extract(Context context, HttpResponseMessage response)
    {
        // Registered relation types compare case-insensitively, extension relation types (URIs) exactly.
        var comparer = Rel.Contains(':') ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;

        var targets = LinkHeaderValue.Parse(response)
            .Where(link => link.Rel?.Split(' ', StringSplitOptions.RemoveEmptyEntries).Contains(Rel, comparer) ?? false)
            .Select(link => new Uri(response.RequestMessage!.RequestUri!, link.Url).AbsoluteUri)
            .ToArray();

        context.Log<LinkExtractor>("Extract: [{Rel}] = [{Targets}]", Rel, targets);

        return targets;
    }
}
