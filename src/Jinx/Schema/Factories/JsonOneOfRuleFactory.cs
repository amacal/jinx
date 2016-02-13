using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonOneOfRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonArray>("oneOf"))
            {
                JsonArray allOf = definition.Get<JsonArray>("oneOf");
                JsonOneOfRule rule = new JsonOneOfRule();

                foreach (JsonObject item in allOf.Items<JsonObject>())
                    rule.Add(parse(item));

                rules.Add(rule);
            }
        }
    }
}