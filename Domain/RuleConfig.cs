using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NRules.Samples.JsonRules.Domain;

//public class RuleCondition
//{
//    public string Attribute { get; set; }  // The attribute to check (e.g., Currency, AssetClass)
//    public string Operator { get; set; }   // The comparison operator (e.g., Equals, Between)
//    public object Value { get; set; }      // The value to compare against (e.g., "USD", or range [2020, 2030])

//    public RuleCondition(string attribute, string op, object value)
//    {
//        Attribute = attribute;
//        Operator = op;
//        Value = value;
//    }
//}

//public class Rule
//{
//    public List<RuleCondition> Conditions { get; set; }  // List of conditions for AND logic

//    public Rule(List<RuleCondition> conditions)
//    {
//        Conditions = conditions;
//    }

//    public void Test()
//    {

//    }
//}

//public class RuleEngine
//{
//    public Func<Position, bool> BuildLambda(Rule rule)
//    {
//        Parameter for the Position object

//       var param = Expression.Parameter(typeof(Position), "position");

//        Start with a null expression that we will build on
//        Expression finalExpression = null;

//        foreach (var condition in rule.Conditions)
//        {
//            Expression conditionExpression = null;
//            var member = Expression.Property(param, condition.Attribute);

//            switch (condition.Operator)
//            {
//                case "Equals":
//                    conditionExpression = Expression.Equal(member, Expression.Constant(condition.Value));
//                    break;

//                case "Between":
//                    var range = (int[])condition.Value;
//                    var lowerBound = Expression.GreaterThanOrEqual(member, Expression.Constant(range[0]));
//                    var upperBound = Expression.LessThanOrEqual(member, Expression.Constant(range[1]));
//                    conditionExpression = Expression.AndAlso(lowerBound, upperBound);
//                    break;
//            }

//            Combine conditions using AND logic
//            finalExpression = finalExpression == null
//                ? conditionExpression
//                : Expression.AndAlso(finalExpression, conditionExpression);
//        }

//        Create the final lambda expression
//        return Expression.Lambda<Func<Position, bool>>(finalExpression, param).Compile();
//    }
//}