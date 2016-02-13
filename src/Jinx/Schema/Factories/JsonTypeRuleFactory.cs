using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonTypeRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonText>("type"))
            {
                JsonText type = definition.Get<JsonText>("type");
                JsonTypeRule rule = new JsonTypeRule();

                rule.AddType(type.Value);
                rules.Add(rule);
            }

            if (definition.Contains<JsonArray>("type"))
            {
                JsonArray type = definition.Get<JsonArray>("type");
                JsonTypeRule rule = new JsonTypeRule();

                foreach (JsonText item in type.Items<JsonText>())
                    rule.AddType(item.Value);

                rules.Add(rule);
            }
        }
    }
}