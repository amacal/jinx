using Jinx.Dom;
using Jinx.Path.Segments;
using System.Collections.Generic;
using Jinx.Path;

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
            items.Add(property, rule);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            bool succeeded = true;
            JsonObject target = value as JsonObject;

            if (target == null)
                return true;

            foreach (string property in target.GetKeys())
                if (items.ContainsKey(property))
                {
                    JsonPathSegment segment = new JsonPropertySegment(property);
                    JsonSchemaCallback scope = callback.Scope(segment);

                    if (items[property].IsValid(definitions, target.Get(property), scope) == false)
                    {
                        callback.Call(segment, value, "The property is not valid.");
                        callback.Add(scope);
                        succeeded = false;
                    }
                }

            return succeeded;
        }
    }
}