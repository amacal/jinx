using Jinx.Dom;
using Jinx.Path.Segments;
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
            bool succeeded = true;
            JsonObject target = value as JsonObject;

            if (target == null)
                return true;

            foreach (string property in properties)
            {
                if (target.Contains(property) == false)
                {
                    callback.Fail(new JsonPropertySegment(property), value, "The property is required.");
                    succeeded = false;
                }
            }

            return succeeded;
        }
    }
}