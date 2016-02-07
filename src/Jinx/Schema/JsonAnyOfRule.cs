using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema
{
    public class JsonAnyOfRule : JsonSchemaRule
    {
        private readonly List<JsonSchemaRule> items;

        public JsonAnyOfRule()
        {
            this.items = new List<JsonSchemaRule>();
        }

        public JsonAnyOfRule(IEnumerable<JsonSchemaRule> items)
        {
            this.items = new List<JsonSchemaRule>(items);
        }

        public void Add(JsonSchemaRule item)
        {
            items.Add(item);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            foreach (JsonSchemaRule rule in items)
                if (rule.IsValid(definitions, value))
                    return true;

            return false;
        }
    }
}
