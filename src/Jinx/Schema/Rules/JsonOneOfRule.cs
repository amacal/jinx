using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonOneOfRule : JsonSchemaRule
    {
        private readonly List<JsonSchemaRule> rules;

        public JsonOneOfRule()
        {
            this.rules = new List<JsonSchemaRule>();
        }

        public void Add(JsonSchemaRule rule)
        {
            rules.Add(rule);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            int count = 0;

            foreach (JsonSchemaRule rule in rules)
            {
                if (rule.IsValid(definitions, value, callback))
                    count++;

                if (count > 1)
                    return false;
            }

            if (count == 0)
                return false;

            return true;
        }
    }
}