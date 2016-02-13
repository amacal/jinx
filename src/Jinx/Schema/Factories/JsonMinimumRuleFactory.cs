using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonMinimumRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonNumber>("minimum"))
            {
                JsonNumber minimum = definition.Get<JsonNumber>("minimum");
                JsonTrue exclusiveMinimum = definition.Get<JsonTrue>("exclusiveMinimum");
                JsonMinimumRule rule = new JsonMinimumRule(minimum.ToDecimal(), exclusiveMinimum != null);

                rules.Add(rule);
            }
        }
    }
}