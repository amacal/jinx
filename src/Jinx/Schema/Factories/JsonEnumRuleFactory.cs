using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonEnumRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonArray>("enum"))
            {
                JsonArray enums = definition.Get<JsonArray>("enum");
                JsonEnumRule rule = new JsonEnumRule();

                foreach (JsonValue value in enums.Items())
                    rule.Add(value);

                rules.Add(rule);
            }
        }
    }
}