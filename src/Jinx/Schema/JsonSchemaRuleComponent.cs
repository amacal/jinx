using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema
{
    public class JsonSchemaRuleComponent : JsonSchemaRule
    {
        private readonly List<JsonSchemaRule> rules;

        public JsonSchemaRuleComponent()
        {
            this.rules = new List<JsonSchemaRule>();
        }

        public void Add(JsonSchemaRule rule)
        {
            this.rules.Add(rule);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            foreach (JsonSchemaRule rule in rules)
                if (rule.IsValid(definitions, value, callback) == false)
                    return false;

            return true;
        }
    }
}