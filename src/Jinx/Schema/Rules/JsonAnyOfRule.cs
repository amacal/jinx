using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonAnyOfRule : JsonSchemaRule
    {
        private readonly List<JsonSchemaRule> rules;

        public JsonAnyOfRule()
        {
            this.rules = new List<JsonSchemaRule>();
        }

        public void Add(JsonSchemaRule rule)
        {
            rules.Add(rule);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            foreach (JsonSchemaRule rule in rules)
                if (rule.IsValid(definitions, value, callback))
                    return true;

            return callback.Call("", value, "At least one schema should be valid");
        }
    }
}