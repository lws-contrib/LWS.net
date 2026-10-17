using VDS.RDF.Parsing;

namespace Model.NewModel;

internal static class Vocabulary
{
    private static readonly NodeFactory factory = new();

    internal static string NS => "http://example.com/";

    internal static INode Tests { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}tests"));

    internal static INode Include { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}include"));

    internal static INode StepName { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}stepName"));

    internal static INode ManifestName { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}manifestName"));

    internal static INode TestName { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}testName"));

    internal static INode Steps { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}steps"));

    internal static INode Request { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}request"));

    internal static INode Headers { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}headers"));

    internal static INode RdfType { get; } = factory.CreateUriNode(UriFactory.Create(RdfSpecsHelper.RdfType));

    internal static INode Body { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}body"));

    internal static INode BodyExtractor { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}BodyExtractor"));

    internal static INode Method { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}method"));

    internal static INode Uri { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}uri"));

    internal static INode Base { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}base"));

    internal static INode Relative { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}relative"));

    internal static INode Value { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}value"));

    internal static INode EqualsValue { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}equals"));

    internal static INode Constant { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}constant"));

    internal static INode Extract { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}extract"));

    internal static INode To { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}to"));

    internal static INode Variable { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}variable"));

    internal static INode Path { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}path"));

    internal static INode Link { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}link"));

    internal static INode Header { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}header"));

    internal static INode Key { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}key"));

    internal static INode Assert { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}assert"));

    internal static INode Contains { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}contains"));

    internal static INode Regex { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}regex"));

    internal static INode StatusCodeExtractor { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}StatusCodeExtractor"));

    internal static INode That { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}that"));

    internal static INode Single { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}single"));

    internal static INode Count { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}count"));

    internal static INode GreaterThan { get; } = factory.CreateUriNode(UriFactory.Create($"{NS}greaterThan"));
}
