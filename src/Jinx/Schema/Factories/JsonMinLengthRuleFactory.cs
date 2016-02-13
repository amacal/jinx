using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonMinLengthRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonNumber>("minLength"))
            {
                JsonNumber minLength = definition.Get<JsonNumber>("minLength");
                JsonMinLengthRule rule = new JsonMinLengthRule(minLength.ToInt32());

                rules.Add(rule);
            }
        }
    }
}