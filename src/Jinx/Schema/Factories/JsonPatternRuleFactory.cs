using Jinx.Dom;
using Jinx.Schema.Rules;
using System;

namespace Jinx.Schema.Factories
{
    public class JsonPatternRuleFactory : JsonSchemaRuleFactory
    {
        public override void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse)
        {
            if (definition.Contains<JsonText>("pattern"))
            {
                JsonText pattern = definition.Get<JsonText>("pattern");
                JsonPatternRule rule = new JsonPatternRule(pattern.Value);

                rules.Add(rule);
            }
        }
    }
}