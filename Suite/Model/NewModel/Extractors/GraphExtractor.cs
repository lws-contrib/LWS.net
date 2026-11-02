using VDS.RDF.Parsing;

namespace Model.NewModel.Extractors;

public partial class GraphExtractor
{
    protected override async Task<object> Extract(Context context, HttpResponseMessage response)
    {
        var mediaType = response.Content.Headers.ContentType?.MediaType;
        var baseUri = response.RequestMessage!.RequestUri!;
        using var reader = new StreamReader(await response.Content.ReadAsStreamAsync());

        var graph = new VDS.RDF.Graph { BaseUri = baseUri };

        switch (mediaType?.ToLowerInvariant())
        {
            case "application/lws+json" or "application/ld+json" or "application/json":
                var store = new TripleStore();
                Resources.JsonLdParser(baseUri).Load(store, reader);

                foreach (var named in store.Graphs.Where(g => g.Name is not null && !g.IsEmpty))
                {
                    throw new InvalidOperationException($"JSON-LD body has a non-empty named graph [{named.Name}].");
                }

                foreach (var source in store.Graphs.Where(g => g.Name is null))
                {
                    graph.Merge(source);
                }

                break;

            default:
                MimeTypesHelper.GetParser(mediaType ?? string.Empty).Load(graph, reader);

                break;
        }

        context.Log<GraphExtractor>("Extract: media type = [{MediaType}], base = [{Base}], triples = [{Count}]", mediaType, baseUri, graph.Triples.Count);

        return graph;
    }
}
