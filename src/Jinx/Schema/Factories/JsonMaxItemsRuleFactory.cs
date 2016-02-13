using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonMaxItemsRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonNumber>("maxItems"))
            {
                JsonNumber maxItems = definition.Get<JsonNumber>("maxItems");
                JsonMaxItemsRule rule = new JsonMaxItemsRule(maxItems.ToInt32());

                rules.Add(rule);
            }
        }
    }
}