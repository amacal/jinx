using Jinx.Dom;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Jinx.Schema
{
    public class JsonObjectRule : JsonSchemaRule
    {
        private readonly Dictionary<string, JsonSchemaRule> properties;
        private readonly Dictionary<string, JsonSchemaRule> patterns;
        private readonly HashSet<string> required;
        private bool additional;

        public JsonObjectRule()
        {
            this.properties = new Dictionary<string, JsonSchemaRule>();
            this.patterns = new Dictionary<string, JsonSchemaRule>();
            this.required = new HashSet<string>();
            this.additional = true;
        }

        public void AddProperty(string name, JsonSchemaRule rule)
        {
            properties[name] = rule;
        }

        public void AddPattern(string name, JsonSchemaRule rule)
        {
            patterns[name] = rule;
        }

        public void AddRequired(string name)
        {
            required.Add(name);
        }

        public void SetAdditional(bool value)
        {
            additional = value;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            List<string> matched = new List<string>(properties.Keys);
            JsonObject objekt = value as JsonObject;

            if (objekt == null)
                return false;

            foreach (string property in properties.Keys)
            {
                JsonSchemaRule rule = properties[property];
                JsonValue target = objekt.Get(property);

                if (target != null && rule.IsValid(definitions, target) == false)
                    return false;

                matched.Remove(property);
            }

            foreach (string property in objekt.GetKeys())
            {
                JsonSchemaRule rule = null;
                Match match = null;

                foreach (string pattern in patterns.Keys)
                {
                    match = Regex.Match(property, pattern);
                    if (match != null)
                    {
                        rule = patterns[pattern];
                        break;
                    }
                }

                if (match != null && rule != null)
                {
                    matched.Remove(property);
                }
            }

            foreach (string property in required)
            {
                JsonValue target = objekt.Get(property);

                if (target == null)
                    return false;
            }

            if (additional == false && matched.Count > 0)
                return false;

            return true;
        }
    }
}