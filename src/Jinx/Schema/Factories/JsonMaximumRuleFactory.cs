using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonMaximumRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonNumber>("maximum"))
            {
                JsonNumber maximum = definition.Get<JsonNumber>("maximum");
                JsonTrue exclusiveMaximum = definition.Get<JsonTrue>("exclusiveMaximum");
                JsonMaximumRule rule = new JsonMaximumRule(maximum.ToDecimal(), exclusiveMaximum != null);

                rules.Add(rule);
            }
        }
    }
}