using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model.EARL;
using Model.NewModel;
using System.Diagnostics;

namespace Model;

public class Executor(ILoggerFactory loggerFactory, IOptions<SuiteOptions> options, HttpClient client)
{
    internal static readonly ActivitySource ActivitySource = new(typeof(Executor).FullName!);

    public static IEnumerable<NewModel.TestCase> Tests => Resources.Graph.Manifest.Tests;

    public async Task<EarlGraph> Execute()
    {
        var graph = new EarlGraph(new VDS.RDF.Graph());

        var assertor = Assertor.Create(graph);
        assertor.Title = "NAME OF ASSERTOR"; // TODO: Don't hardcode

        var suiteRequirement = TestRequirement.Create(graph);
        suiteRequirement.Title = Resources.Graph.Manifest.Name;

        await Execute(Resources.Graph.Manifest, graph, assertor, suiteRequirement);

        return graph;
    }

    private async Task Execute(Manifest manifest, EarlGraph graph, Assertor assertor, TestRequirement suiteRequirement)
    {
        using var activity = ActivitySource.StartActivity(manifest.Name)?.SetTag("test.suite.name", manifest.Name);

        foreach (var testCase in manifest.Tests)
        {
            var result3 = await Execute(testCase);
            var entryRequirement = TestRequirement.Create(graph);
            entryRequirement.Title = testCase.Name;
            entryRequirement.IsPartOf = suiteRequirement;

            var assertion = EARL.Assertion.Create(graph);
            assertion.AssertedBy = assertor;

            var test = assertion.Test = EARL.TestCase.Create(graph);
            test.Title = testCase.Name;
            test.IsPartOf = entryRequirement;

            var result = assertion.Result = TestResult.Create(graph);
            result.Info = result3.Info;
            result.Outcome = result3.Outcome switch
            {
                "Pass" => EARL.Vocabulary.Passed,
                "Fail" => EARL.Vocabulary.Failed,
                "Inconclusive" => EARL.Vocabulary.CantTell,
                _ => throw new InvalidOperationException($"Unknown outcome: {result3.Outcome}")
            };
        }

        foreach (var include in manifest.Include)
        {
            await Execute(Resources.Parse(new(manifest.Graph.BaseUri, include)).Manifest, graph, assertor, suiteRequirement);
        }
    }

    public async Task<Result> Execute(string name, ActivityContext parentContext = default)
    {
        var test = Tests.Single(test => test.Name == name);
        return await Execute(test, parentContext);
    }

    private async Task<Result> Execute(NewModel.TestCase test, ActivityContext parentContext = default)
    {
        using var activity = ActivitySource.StartActivity(test.Name, ActivityKind.Internal, parentContext)?.SetTag("test.case.name", test.Name);

        var result = await test.Execute(new Context(loggerFactory, options, client));

        activity?.SetTag("test.case.result.status", result.Outcome.ToLowerInvariant());

        if (result.Outcome == "Fail")
        {
            activity?.SetStatus(ActivityStatusCode.Error, result.Info);
        }

        return result;
    }
}
