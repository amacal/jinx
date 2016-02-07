using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonPropertiesRule : JsonSchemaRule
    {
        private readonly Dictionary<string, JsonSchemaRule> items;

        public JsonPropertiesRule()
        {
            this.items = new Dictionary<string, JsonSchemaRule>();
        }

        public void AddProperty(string property, JsonSchemaRule rule)
        {
            items[property] = rule;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            JsonObject target = value as JsonObject;

            if (target == null)
                return true;

            foreach (string property in target.GetKeys())
                if (items.ContainsKey(property))
                    if (items[property].IsValid(definitions, target.Get(property)) == false)
                        return false;

            return true;
        }
    }
}