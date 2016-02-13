using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonItemsRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonObject>("items"))
            {
                JsonObject items = definition.Get<JsonObject>("items");
                JsonItemsRule rule = new JsonItemsRule(parse(items));

                rules.Add(rule);
            }

            if (definition.Contains<JsonArray>("items"))
            {
                JsonArray items = definition.Get<JsonArray>("items");
                JsonItemsRule rule = new JsonItemsRule();

                foreach (JsonObject item in items.Items<JsonObject>())
                    rule.AddTuple(parse(item));

                rules.Add(rule);
            }
        }
    }
}