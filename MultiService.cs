using NRules.Samples.JsonRules.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRules.Samples.JsonRules;

public class MultiService
{
    //public void Execute()
    //{
    //    // Example position data
    //    var position = new Position("USD", "Equity", 2022, 2025);

    //    // Example rules with multiple AND conditions
    //    var rule = new Rule(new List<RuleCondition>
    //    {
    //        new RuleCondition("Currency", "Equals", "USD"),
    //        new RuleCondition("AssetClass", "Equals", "Equity"),
    //        new RuleCondition("YearMaturityFrom", "Between", new int[] { 2020, 2030 })
    //    });


    //    // Build the lambda
    //    var ruleEngine = new RuleEngine();
    //    var ruleFunc = ruleEngine.BuildLambda(rule);

    //    // Evaluate the rule using the generated lambda
    //    bool isMatch = ruleFunc(position);

    //    Console.WriteLine($"Rule matched: {isMatch}");
    //}
}
