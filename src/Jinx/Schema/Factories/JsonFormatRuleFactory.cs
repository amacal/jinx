using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonFormatRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonText>("format"))
            {
                JsonText format = definition.Get<JsonText>("format");
                JsonFormatRule rule = new JsonFormatRule(format.Value);

                rules.Add(rule);
            }
        }
    }
}