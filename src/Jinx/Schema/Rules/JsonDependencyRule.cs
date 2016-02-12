using Jinx.Dom;
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
            JsonObject target = value.As<JsonObject>();

            if (target == null)
                return true;

            foreach (string property in byName.Keys)
                if (target.Contains(property))
                    foreach (string dependency in byName[property])
                        if (target.Contains(dependency) == false)
                            return callback.Call($".{property}", value, "The dependency is not valid.");

            foreach (string property in byRule.Keys)
                if (target.Contains(property))
                    if (byRule[property].IsValid(definitions, value, callback) == false)
                        return callback.Call($".{property}", value, "The dependency is not valid.");

            return true;
        }
    }
}