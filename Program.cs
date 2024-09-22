using NRules.Diagnostics;
using NRules.Fluent;
using NRules.Json;
using NRules.RuleModel;
using NRules.RuleModel.Builders;
using NRules.Samples.JsonRules.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;

namespace NRules.Samples.JsonRules;

internal class Program
{
    private static void Main()
    {
        //var stopwatch = new Stopwatch();
        //stopwatch.Start();

        //MultiService service = new MultiService();
        //service.Execute();

        //stopwatch.Stop();
        //Console.WriteLine($"Multithread execution time: {stopwatch.ElapsedMilliseconds}ms");

        //var nrule = new NruleService();
        //nrule.Execute();

        Execute100000NRules();
    }

    private static void ExecuteNRules()
    {
        // Create and configure the rule repository
        var repository = new RuleRepository();

        repository.Load(x => x.From(typeof(UsdAssetClassRule).Assembly));

        // Create the rule engine
        var factory = repository.Compile();
        var engine = factory.CreateSession();

        // Insert facts into the engine
        var position = new Position
        {
            Currency = "SGD",
            AssetClass = "Equity",
            YearMaturityFrom = 2020,
            YearMaturityTo = 2028
        };
        engine.Insert(position);

        // Fire the rules
        engine.Fire();

        // Retrieve results (assuming you are tracking them)
        var results = new List<Result>();
        foreach (var fact in engine.Query<Result>())
        {
            results.Add(fact);
        }

        // Output the results
        foreach (var result in results)
        {
            Console.WriteLine($"Result Value: {result.Value}");
        }
    }

    private static void Execute100000NRules()
    {  
        var repository = new CustomRuleRepository();
        repository.LoadRules();
        ISessionFactory factory = repository.Compile();

        ISession session = factory.CreateSession();

        // Insert facts into the engine
        var position = new Position
        {
            Currency = "SGD",
            AssetClass = "Equity",
            YearMaturityFrom = 2020,
            YearMaturityTo = 2028
        };
        session.Insert(position);


        session.Events.RuleFiredEvent += OnRuleFiringEvent;
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Fire the rules
        session.Fire();

   

        ReteGraph reteGraph = session.GetSchema();
        //ReteNode node = reteGraph.Nodes.First(n => n.NodeType == NodeType.BetaMemory);
        //INodeMetrics nodeMetrics = session.Metrics.FindByNodeId(node.Id);
        //long totalTimeSpentMsec = nodeMetrics.InsertDurationMilliseconds +
        //                          nodeMetrics.UpdateDurationMilliseconds +
        //                          nodeMetrics.RetractDurationMilliseconds;
        //Console.WriteLine($"Node {node.Id}:{node.NodeType}. ElementCount={nodeMetrics.ElementCount}, TotalTimeSpentMsec={totalTimeSpentMsec}");

        stopwatch.Stop();
        Console.WriteLine($"Multithread execution time: {stopwatch.ElapsedMilliseconds}ms");

        //// Retrieve results (assuming you are tracking them)
        //var results = new List<Result>();
        //foreach (var fact in session.Query<Result>())
        //{
        //    results.Add(fact);
        //}

        //// Output the results
        //foreach (var result in results)
        //{
        //    Console.WriteLine($"Result Value: {result.Value}");
        //}

    }

    private static void OnRuleFiringEvent(object sender, AgendaEventArgs e)
    {
        Console.WriteLine("Rule about to fire {0}", e.Rule.Name);
    }
}

public class CustomRuleRepository : IRuleRepository
{
    private readonly IRuleSet _ruleSet = new RuleSet("MyRuleSet");

    public IEnumerable<IRuleSet> GetRuleSets()
    {
        return new[] { _ruleSet };
    }

    public void LoadRules()
    {
        // Create 10,000 rules and add them to the repository
        for (int i = 0; i < 10; i++)
        {
            var rule = BuildRule(i);
            _ruleSet.Add(rule);
        }

        var rule2 = BuildRule2(10);
        _ruleSet.Add(rule2);
    }

    private IRuleDefinition BuildRule(int index)
    {
        // Create a rule builder
        var ruleBuilder = new RuleBuilder();

        // Define the rule name and description
        ruleBuilder.Name("UsdAssetClassRule" + index);
        ruleBuilder.Description("Matches positions with USD currency and specific asset classes.");

        // Define LeftHandSide (LHS) patterns and conditions

        // 1. Position pattern (position.Currency == "USD" && (position.AssetClass == "Equity" || position.AssetClass == "Bonds"))
        PatternBuilder positionPattern = ruleBuilder.LeftHandSide().Pattern(typeof(Position), "position");
        ParameterExpression positionParameter = positionPattern.Declaration.ToParameterExpression();

        // Create condition: position.Currency == "USD"
        var currencyCondition = Expression.Lambda(
            Expression.Equal(
                Expression.Property(positionParameter, nameof(Position.Currency)),
                Expression.Constant("USD")),
            positionParameter);

        positionPattern.Condition(currencyCondition);

        // Create condition: position.AssetClass == "Equity" || position.AssetClass == "Bonds"
        var assetClassCondition = Expression.Lambda(
            Expression.OrElse(
                Expression.Equal(
                    Expression.Property(positionParameter, nameof(Position.AssetClass)),
                    Expression.Constant("Equity")),
                Expression.Equal(
                    Expression.Property(positionParameter, nameof(Position.AssetClass)),
                    Expression.Constant("Bonds"))),
            positionParameter);

        positionPattern.Condition(assetClassCondition);

        //Build actions
        Expression<Action<IContext>> action =
            (ctx) => ctx.Result(0.8m);
        ruleBuilder.RightHandSide().Action(action);

        // Build and return the rule (this can be added to a repository)
        var rule = ruleBuilder.Build();

        ////Set up JsonSerializerOptions
        //var options = new JsonSerializerOptions
        //{
        //    WriteIndented = true,
        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //};
        //RuleSerializer.Setup(options);

        ////Serialize a rule into JSON
        //var json = JsonSerializer.Serialize(rule, options);

        return rule;
    }

    private IRuleDefinition BuildRule2(int index)
    {
        // Create a rule builder
        var ruleBuilder = new RuleBuilder();

        // Define the rule name and description
        ruleBuilder.Name("UsdAssetClassRule" + index);
        ruleBuilder.Description("Matches positions with USD currency and specific asset classes.");

        // Define LeftHandSide (LHS) patterns and conditions

        // 1. Position pattern (position.Currency == "USD" && (position.AssetClass == "Equity" || position.AssetClass == "Bonds"))
        PatternBuilder positionPattern = ruleBuilder.LeftHandSide().Pattern(typeof(Position), "position");
        ParameterExpression positionParameter = positionPattern.Declaration.ToParameterExpression();

        // Create condition: position.Currency == "USD"
        var currencyCondition = Expression.Lambda(
            Expression.Equal(
                Expression.Property(positionParameter, nameof(Position.Currency)),
                Expression.Constant("SGD")),
            positionParameter);

        positionPattern.Condition(currencyCondition);

        // Create condition: position.AssetClass == "Equity" || position.AssetClass == "Bonds"
        var assetClassCondition = Expression.Lambda(
            Expression.OrElse(
                Expression.Equal(
                    Expression.Property(positionParameter, nameof(Position.AssetClass)),
                    Expression.Constant("Equity")),
                Expression.Equal(
                    Expression.Property(positionParameter, nameof(Position.AssetClass)),
                    Expression.Constant("Derivative"))),
            positionParameter);

        positionPattern.Condition(assetClassCondition);

        //Build actions
        Expression<Action<IContext>> action =
            (ctx) => ctx.Result(0.8m);
        ruleBuilder.RightHandSide().Action(action);

        // Build and return the rule (this can be added to a repository)
        var rule = ruleBuilder.Build();

        ////Set up JsonSerializerOptions
        //var options = new JsonSerializerOptions
        //{
        //    WriteIndented = true,
        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //};
        //RuleSerializer.Setup(options);

        ////Serialize a rule into JSON
        //var json = JsonSerializer.Serialize(rule, options);

        return rule;
    }
}