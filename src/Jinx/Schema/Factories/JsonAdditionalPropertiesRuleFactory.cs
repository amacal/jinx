using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonAdditionalPropertiesRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonFalse>("additionalProperties"))
            {
                JsonObject properties = definition.Get<JsonObject>("properties");
                JsonObject patterns = definition.Get<JsonObject>("patternProperties");
                JsonAdditionalPropertiesRule rule = new JsonAdditionalPropertiesRule();

                if (properties != null)
                    foreach (string property in properties.GetKeys())
                        rule.AddProperty(property);

                if (patterns != null)
                    foreach (string pattern in patterns.GetKeys())
                        rule.AddPattern(pattern);

                rules.Add(rule);
            }

            if (definition.Contains<JsonObject>("additionalProperties"))
            {
                JsonObject additionalProperties = definition.Get<JsonObject>("additionalProperties");
                JsonObject properties = definition.Get<JsonObject>("properties");
                JsonObject patterns = definition.Get<JsonObject>("patternProperties");
                JsonAdditionalPropertiesRule rule = new JsonAdditionalPropertiesRule(parse(additionalProperties));

                if (properties != null)
                    foreach (string property in properties.GetKeys())
                        rule.AddProperty(property);

                if (patterns != null)
                    foreach (string pattern in patterns.GetKeys())
                        rule.AddPattern(pattern);

                rules.Add(rule);
            }
        }
    }
}