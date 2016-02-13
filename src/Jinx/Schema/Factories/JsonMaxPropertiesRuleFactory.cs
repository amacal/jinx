using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonMaxPropertiesRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonNumber>("maxProperties"))
            {
                JsonNumber maxProperties = definition.Get<JsonNumber>("maxProperties");
                JsonMaxPropertiesRule rule = new JsonMaxPropertiesRule(Int32.Parse(maxProperties.Value));

                rules.Add(rule);
            }
        }
    }
}