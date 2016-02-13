using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonAllOfRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonArray>("allOf"))
            {
                JsonArray allOf = definition.Get<JsonArray>("allOf");
                JsonAllOfRule rule = new JsonAllOfRule();

                foreach (JsonObject item in allOf.Items<JsonObject>())
                    rule.Add(parse(item));

                rules.Add(rule);
            }
        }
    }
}