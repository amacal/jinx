using Jinx.Dom;
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
            JsonObject target = value as JsonObject;

            if (target == null)
                return true;

            foreach (Regex pattern in items.Keys)
                foreach (string property in target.GetKeys().Where(x => pattern.IsMatch(x)))
                    if (items[pattern].IsValid(definitions, target.Get(property), callback) == false)
                        return callback.Call(new JsonPropertySegment(property), value, "The property is not valid.");

            return true;
        }
    }
}