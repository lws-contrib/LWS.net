using Model;
using Model.NewModel;

namespace Test;

[TestClass]
public sealed class TestSuite
{
    private static IEnumerable<TestDataRow<TestCase>> TestCases =>
        Executor.Tests.Select(test =>
            new TestDataRow<TestCase>(test)
            {
                DisplayName = test.Name
                // TODO: ignore
                // TODO: categories
            });

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
