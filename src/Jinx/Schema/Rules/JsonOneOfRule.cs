using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonOneOfRule : JsonSchemaRule
    {
        private readonly List<JsonSchemaRule> rules;

        public JsonOneOfRule()
        {
            this.rules = new List<JsonSchemaRule>();
        }

        public void Add(JsonSchemaRule rule)
        {
            rules.Add(rule);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            int count = 0;
            List<JsonSchemaCallback> scopes = new List<JsonSchemaCallback>();

            foreach (JsonSchemaRule rule in rules)
            {
                JsonSchemaCallback scope = callback.Scope();

                if (rule.IsValid(definitions, value, scope))
                    count++;

                if (scope.Count > 0)
                    scopes.Add(scope);

                if (count > 1)
                    break;
            }

            if (count == 1)
                return true;

            if (count > 1)
                return callback.Fail(value, $"Exactly one schema should be valid, but {count} schemas were valid.");

            foreach (JsonSchemaCallback scope in scopes)
                callback.Add(scope);

            return callback.Fail(value, "Exactly one schema should be valid, but nothing was valid.");
        }
    }
}