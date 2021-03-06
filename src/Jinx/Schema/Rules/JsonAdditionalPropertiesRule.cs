﻿using Jinx.Dom;
using Jinx.Path;
using Jinx.Path.Segments;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Jinx.Schema.Rules
{
    public class JsonAdditionalPropertiesRule : JsonSchemaRule
    {
        private readonly JsonSchemaRule rule;
        private readonly List<string> properties;
        private readonly List<Regex> patterns;

        public JsonAdditionalPropertiesRule()
        {
            this.properties = new List<string>();
            this.patterns = new List<Regex>();
        }

        public JsonAdditionalPropertiesRule(JsonSchemaRule rule)
        {
            this.rule = rule;
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

            bool succeeded = true;
            List<string> left = new List<string>(target.GetKeys());

            foreach (string property in properties)
                left.Remove(property);

            foreach (Regex pattern in patterns)
                left.RemoveAll(pattern.IsMatch);

            if (left.Count == 0)
                return true;

            if (rule == null && left.Count > 0)
            {
                foreach (string property in left)
                    callback.Fail(new JsonPropertySegment(property), value, "The presence of any additional property is not allowed.");

                return false;
            }

            if (rule != null)
            {
                foreach (string property in left)
                {
                    JsonPathSegment segment = new JsonPropertySegment(property);
                    JsonSchemaCallback scope = callback.Scope(segment);

                    if (rule.IsValid(definitions, target.Get(property), scope) == false)
                    {
                        callback.Add(scope);
                        succeeded = false;
                    }
                }
            }

            return succeeded;
        }
    }
}