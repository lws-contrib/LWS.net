using Model;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace ModelTest;

[TestClass]
public class TemplateExpanderTest
{
    private const string Prefixes = "@prefix : <http://example.com/> . @prefix p: <urn:lws:param:> . @prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> . ";

    private const string Template = """
        :CreateContainer
            a :StepTemplate ;
            :parameter (
                [ :parameterName "in" ; :required true ]
                [ :parameterName "as" ; :default "containerLocation" ]
            ) ;
            :expansion [
                :request [
                    :method "POST" ;
                    :uri p:in ;
                    :headers [
                        :key "Link" ;
                        :value [ :constant "<https://www.w3.org/ns/lws#Container>; rel=\"type\"" ] ;
                    ] ;
                ] ;
                :extract (
                    [ :header "Location" ; :to [ :param "as" ] ]
                ) ;
            ] ;
        .
        """;

    private static string Step(string name, string uri, string to) => $$"""
        [
            :stepName "{{name}}" ;
            :request [
                :method "POST" ;
                :uri {{uri}} ;
                :headers [
                    :key "Link" ;
                    :value [ :constant "<https://www.w3.org/ns/lws#Container>; rel=\"type\"" ] ;
                ] ;
            ] ;
            :extract (
                [ :header "Location" ; :to "{{to}}" ]
            ) ;
        ] .
        """;

    [TestMethod]
    public void AppliesDefault()
    {
        var actual = Expand($"{Template} _:s a :CreateContainer ; :stepName \"Create\" ; :argument [ :parameterName \"in\" ; :value _:storageRoot ] .");

        GraphAssert.Isomorphic($"{Prefixes}{Step("Create", "_:storageRoot", "containerLocation")}", actual);
    }

    [TestMethod]
    public void AppliesExplicitArgument()
    {
        var actual = Expand($"{Template} _:s a :CreateContainer ; :stepName \"Create\" ; :argument [ :parameterName \"in\" ; :value _:storageRoot ], [ :parameterName \"as\" ; :value \"other\" ] .");

        GraphAssert.Isomorphic($"{Prefixes}{Step("Create", "_:storageRoot", "other")}", actual);
    }

    [TestMethod]
    public void GivesInstancesDistinctBlankNodes()
    {
        var actual = Expand($"{Template} _:a a :CreateContainer ; :stepName \"A\" ; :argument [ :parameterName \"in\" ; :value _:r ], [ :parameterName \"as\" ; :value \"x\" ] . _:b a :CreateContainer ; :stepName \"B\" ; :argument [ :parameterName \"in\" ; :value _:r ], [ :parameterName \"as\" ; :value \"y\" ] .");

        var one = Step("A", "_:r", "x");
        var two = Step("B", "_:r", "y");
        GraphAssert.Isomorphic($"{Prefixes}{one} {two}", actual);
    }

    [TestMethod]
    public void LeavesGraphWithoutTemplatesUnchanged()
    {
        const string input = "@prefix : <http://example.com/> . _:a :stepName \"A\" ; :request [ :method \"GET\" ] .";

        GraphAssert.Isomorphic(input, Expand(input));
    }

    [TestMethod]
    public void RejectsMissingRequiredParameter()
    {
        Assert.ThrowsExactly<TemplateExpansionException>(() => Expand($"{Template} _:s a :CreateContainer ."));
    }

    [TestMethod]
    public void RejectsUndeclaredPlaceholder()
    {
        const string template = """
            :T a :StepTemplate ; :parameter () ; :expansion [ :to [ :param "missing" ] ] .
            """;

        Assert.ThrowsExactly<TemplateExpansionException>(() => Expand($"{template} _:s a :T ."));
    }

    [TestMethod]
    public void RejectsTwoValuesForOneParameter()
    {
        Assert.ThrowsExactly<TemplateExpansionException>(() => Expand($"{Template} _:s a :CreateContainer ; :argument [ :parameterName \"in\" ; :value _:a ], [ :parameterName \"in\" ; :value _:b ] ."));
    }

    [TestMethod]
    public void RejectsArgumentForUndeclaredParameter()
    {
        Assert.ThrowsExactly<TemplateExpansionException>(() => Expand($"{Template} _:s a :CreateContainer ; :argument [ :parameterName \"in\" ; :value _:r ], [ :parameterName \"nope\" ; :value _:x ] ."));
    }

    private static IGraph Expand(string turtle)
    {
        var graph = new Graph();
        new TurtleParser().Load(graph, new StringReader(turtle.StartsWith('@') ? turtle : $"{Prefixes}{turtle}"));

        TemplateExpander.Expand(graph);

        return graph;
    }
}
