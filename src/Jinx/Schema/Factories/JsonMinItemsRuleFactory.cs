using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonMinItemsRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonNumber>("minItems"))
            {
                JsonNumber minItems = definition.Get<JsonNumber>("minItems");
                JsonMinItemsRule rule = new JsonMinItemsRule(minItems.ToInt32());

                rules.Add(rule);
            }
        }
    }
}