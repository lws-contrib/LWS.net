using System.Text.RegularExpressions;

namespace Model.NewModel.Assertions;

public partial class RegexAssertion
{
    protected internal override async Task<Result> ExecuteInternal(object input, Context context)
    {
        var actual = input as string ?? throw new InvalidOperationException("Input is not a string.");
        var expected = await Matches.Evaluate(context) as string ?? throw new InvalidOperationException("Value of matches is not a string.");
        return Regex.IsMatch(actual, expected)
            ? new Result { Outcome = "Pass", Info = $"Input matches '{expected}'." }
            : new Result { Outcome = "Fail", Info = $"Input expected to match '{expected}', but got '{actual}'." };
    }
}
