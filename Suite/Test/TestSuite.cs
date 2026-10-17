using Model;
using Model.NewModel;

namespace Test;

[TestClass]
public sealed class TestSuite
{
    private static IEnumerable<TestDataRow<TestCase>> TestCases => Rows(Resources.Graph.Manifest);

    private static IEnumerable<TestDataRow<TestCase>> Rows(Manifest manifest)
    {
        foreach (var test in manifest.Test)
        {
            yield return new TestDataRow<TestCase>(test)
            {
                DisplayName = $"{manifest.ManifestName}: {test.TestName}"
                // TODO: ignore
                // TODO: categories
            };
        }

        foreach (var include in manifest.Include)
        {
            foreach (var row in Rows(Resources.Parse(new(manifest.Graph.BaseUri, include)).Manifest))
            {
                yield return row;
            }
        }
    }

    [TestMethod]
    [DynamicData(nameof(TestCases))]
    public async Task Entry(TestCase testCase)
    {
        var result = await SuiteApplication.Test(testCase);

        switch (result.Outcome)
        {
            case "Fail":
                Assert.Fail(result.Info);
                break;
            case "Inconclusive":
                Assert.Inconclusive(result.Info);
                break;
        }
    }
}
