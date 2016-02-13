using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonRequiredRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonArray>("required"))
            {
                JsonArray required = definition.Get<JsonArray>("required");
                JsonRequiredRule rule = new JsonRequiredRule();

                foreach (JsonText property in required.Items())
                {
                    rule.Add(property.Value);
                }

                rules.Add(rule);
            }
        }
    }
}