using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonAnyOfRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonArray>("anyOf"))
            {
                JsonArray allOf = definition.Get<JsonArray>("anyOf");
                JsonAnyOfRule rule = new JsonAnyOfRule();

                foreach (JsonObject item in allOf.Items<JsonObject>())
                    rule.Add(parse(item));

                rules.Add(rule);
            }
        }
    }
}