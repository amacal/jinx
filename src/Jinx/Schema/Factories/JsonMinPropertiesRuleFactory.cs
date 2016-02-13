using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonMinPropertiesRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonNumber>("minProperties"))
            {
                JsonNumber minProperties = definition.Get<JsonNumber>("minProperties");
                JsonMinPropertiesRule rule = new JsonMinPropertiesRule(Int32.Parse(minProperties.Value));

                rules.Add(rule);
            }
        }
    }
}