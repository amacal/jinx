using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema
{
    public class JsonAllOfRule : JsonSchemaRule
    {
        private readonly List<JsonSchemaRule> items;

        public JsonAllOfRule()
        {
            this.items = new List<JsonSchemaRule>();
        }

        public JsonAllOfRule(IEnumerable<JsonSchemaRule> items)
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
                if (rule.IsValid(definitions, value) == false)
                    return false;

            return true;
        }
    }
}