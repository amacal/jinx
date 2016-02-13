using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonPropertiesRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonObject>("properties"))
            {
                JsonObject properties = definition.Get<JsonObject>("properties");
                JsonPropertiesRule rule = new JsonPropertiesRule();

                foreach (string property in properties.GetKeys())
                    rule.AddProperty(property, parse(properties.Get<JsonObject>(property)));

                rules.Add(rule);
            }
        }
    }
}