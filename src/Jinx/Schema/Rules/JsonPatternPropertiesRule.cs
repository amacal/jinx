using Jinx.Dom;
using Jinx.Path;
using Jinx.Path.Segments;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Jinx.Schema.Rules
{
    public class JsonPatternPropertiesRule : JsonSchemaRule
    {
        private readonly Dictionary<Regex, JsonSchemaRule> items;

        public JsonPatternPropertiesRule()
        {
            this.items = new Dictionary<Regex, JsonSchemaRule>();
        }

        public void AddPattern(string property, JsonSchemaRule rule)
        {
            items.Add(new Regex(property), rule);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            bool succeeded = true;
            JsonObject target = value as JsonObject;

            if (target == null)
                return true;

            foreach (Regex pattern in items.Keys)
            {
                foreach (string property in target.GetKeys().Where(x => pattern.IsMatch(x)))
                {
                    JsonSchemaRule rule = items[pattern];
                    JsonPathSegment segment = new JsonPropertySegment(property);
                    JsonSchemaCallback scope = callback.Scope(segment);

                    if (rule.IsValid(definitions, target.Get(property), callback) == false)
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