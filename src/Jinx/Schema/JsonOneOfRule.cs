using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema
{
    public class JsonOneOfRule : JsonSchemaRule
    {
        private readonly List<JsonSchemaRule> items;

        public JsonOneOfRule()
        {
            this.items = new List<JsonSchemaRule>();
        }

        public void Add(JsonSchemaRule item)
        {
            items.Add(item);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            int counter = 0;

            foreach (JsonSchemaRule rule in items)
                if (rule.IsValid(definitions, value))
                    counter++;

            if (counter == 1)
                return true;

            return false;
        }
    }
}
