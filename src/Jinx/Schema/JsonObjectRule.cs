using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema
{
    public class JsonObjectRule : JsonSchemaRule
    {
        private readonly Dictionary<string, JsonSchemaRule> properties;
        private readonly HashSet<string> required;

        public JsonObjectRule()
        {
            this.properties = new Dictionary<string, JsonSchemaRule>();
            this.required = new HashSet<string>();
        }

        public void AddProperty(string name, JsonSchemaRule rule)
        {
            properties[name] = rule;
        }

        public void AddRequired(string name)
        {
            required.Add(name);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            JsonObject objekt = value as JsonObject;

            if (objekt == null)
                return false;

            foreach (string property in properties.Keys)
            {
                JsonSchemaRule rule = properties[property];
                JsonValue target = objekt.Get(property);

                if (target != null && rule.IsValid(definitions, target) == false)
                    return false;
            }

            foreach (string property in required)
            {
                JsonValue target = objekt.Get(property);

                if (target == null)
                    return false;
            }

            return true;
        }
    }
}
