using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema
{
    public class JsonAllRule : JsonSchemaRule
    {
        private readonly JsonSchemaRule root;
        private readonly List<JsonSchemaRule> items;

        public JsonAllRule(JsonSchemaRule root)
        {
            this.root = root;
            this.items = new List<JsonSchemaRule>();
        }

        public void Add(JsonSchemaRule item)
        {
            items.Add(item);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            if (root.IsValid(definitions, value) == false)
                return false;

            foreach (JsonSchemaRule rule in items)
                if (rule.IsValid(definitions, value) == false)
                    return false;

            return true;
        }
    }
}
