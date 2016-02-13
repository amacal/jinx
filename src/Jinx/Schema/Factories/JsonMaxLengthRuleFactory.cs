using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonMaxLengthRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonNumber>("maxLength"))
            {
                JsonNumber maxLength = definition.Get<JsonNumber>("maxLength");
                JsonMaxLengthRule rule = new JsonMaxLengthRule(maxLength.ToInt32());

                rules.Add(rule);
            }
        }
    }
}