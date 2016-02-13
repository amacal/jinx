using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonPatternPropertiesRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonObject>("patternProperties"))
            {
                JsonObject patterns = definition.Get<JsonObject>("patternProperties");
                JsonPatternPropertiesRule rule = new JsonPatternPropertiesRule();

                foreach (string pattern in patterns.GetKeys())
                    rule.AddPattern(pattern, parse(patterns.Get<JsonObject>(pattern)));

                rules.Add(rule);
            }
        }
    }
}