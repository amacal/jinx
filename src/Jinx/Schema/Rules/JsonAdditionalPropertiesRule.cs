using Jinx.Dom;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Jinx.Schema.Rules
{
    public class JsonAdditionalPropertiesRule : JsonSchemaRule
    {
        private readonly List<string> properties;
        private readonly List<Regex> patterns;

        public JsonAdditionalPropertiesRule()
        {
            this.properties = new List<string>();
            this.patterns = new List<Regex>();
        }

        public void AddProperty(string property)
        {
            properties.Add(property);
        }

        public void AddPattern(string pattern)
        {
            patterns.Add(new Regex(pattern));
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            JsonObject target = value as JsonObject;

            if (target == null)
                return true;

            List<string> left = new List<string>(target.GetKeys());

            foreach (string property in properties)
                left.Remove(property);

            foreach (Regex pattern in patterns)
                left.RemoveAll(pattern.IsMatch);

            if (left.Count > 0)
                return false;

            return true;
        }
    }
}