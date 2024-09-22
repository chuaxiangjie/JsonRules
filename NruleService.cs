using NRules.RuleModel;
using NRules.Fluent.Dsl;
using NRules.Samples.JsonRules.Domain;

namespace NRules.Samples.JsonRules;

public class NruleService
{
    
}

public class UsdAssetClassRule : Rule
{
    public override void Define()
    {
        Position position = null;

        When()
            .Match(() => position, p => p.Currency == "USD", p => (p.AssetClass == "Equity" || p.AssetClass == "Bonds"));
        Then()
            .Do(ctx => ctx.Result(0.8m));
    }
}

public class Result
{
    public decimal Value { get; set; }
}

public static class ContextExtensions
{
    public static void Result(this IContext context, decimal value)
    {
        var result = new Result { Value = value };
        context.Insert(result);
    }
}