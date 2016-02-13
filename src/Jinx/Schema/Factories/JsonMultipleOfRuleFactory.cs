using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonMultipleOfRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonNumber>("multipleOf"))
            {
                JsonNumber multipleOf = definition.Get<JsonNumber>("multipleOf");
                JsonMultipleOfRule rule = new JsonMultipleOfRule(multipleOf.ToDecimal());

                rules.Add(rule);
            }
        }
    }
}