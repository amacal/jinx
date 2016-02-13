using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonNotRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonObject>("not"))
            {
                JsonObject not = definition.Get<JsonObject>("not");
                JsonNotRule rule = new JsonNotRule(parse(not));

                rules.Add(rule);
            }
        }
    }
}