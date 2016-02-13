using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonRefRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonText>("$ref"))
            {
                string reference = definition.Get<JsonText>("$ref").Value;
                JsonRefRule rule = new JsonRefRule(reference);

                rules.Add(rule);
            }
        }
    }
}