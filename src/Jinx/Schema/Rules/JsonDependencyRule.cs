using Jinx.Dom;
using Jinx.Path;
using Jinx.Path.Segments;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonDependencyRule : JsonSchemaRule
    {
        private readonly Dictionary<string, string[]> byName;
        private readonly Dictionary<string, JsonSchemaRule> byRule;

        public JsonDependencyRule()
        {
            this.byName = new Dictionary<string, string[]>();
            this.byRule = new Dictionary<string, JsonSchemaRule>();
        }

        public void Add(string property, string[] dependencies)
        {
            byName.Add(property, dependencies);
        }

        public void Add(string property, JsonSchemaRule rule)
        {
            byRule.Add(property, rule);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            bool succeeded = true;
            JsonObject target = value.As<JsonObject>();

            if (target == null)
                return true;

            foreach (string property in byName.Keys)
                if (target.Contains(property))
                    foreach (string dependency in byName[property])
                        if (target.Contains(dependency) == false)
                        {
                            JsonPathSegment segment = new JsonPropertySegment(property);

                            callback.Fail(segment, value, $"The dependency is not valid. Missing property: {dependency}.");
                            succeeded = false;
                        }

            foreach (string property in byRule.Keys)
            {
                if (target.Contains(property))
                {
                    JsonSchemaRule rule = byRule[property];
                    JsonPathSegment segment = new JsonPropertySegment(property);
                    JsonSchemaCallback scope = callback.Scope(segment);

                    if (rule.IsValid(definitions, value, scope) == false)
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