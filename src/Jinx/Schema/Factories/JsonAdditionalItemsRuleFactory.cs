using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonAdditionalItemsRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parsee)
        {
            if (definition.Contains<JsonFalse>("additionalItems"))
            {
                if (definition.Contains<JsonArray>("items"))
                {
                    JsonArray items = definition.Get<JsonArray>("items");
                    JsonAdditionalItemsRule rule = new JsonAdditionalItemsRule(items.Count);

                    rules.Add(rule);
                }
            }
        }
    }
}