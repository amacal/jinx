using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonRequiredRule : JsonSchemaRule
    {
        private readonly List<string> properties;

        public JsonRequiredRule()
        {
            this.properties = new List<string>();
        }

        public void Add(string property)
        {
            properties.Add(property);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonObject target = value as JsonObject;

            if (target == null)
                return true;

            foreach (string property in properties)
                if (target.Contains(property) == false)
                    return callback.Call("", value, $"The property '{property}' is required");

            return true;
        }
    }
}