using Jinx.Dom;
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

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            JsonObject target = value as JsonObject;

            if (target == null)
                return true;

            foreach (Regex pattern in items.Keys)
                foreach (string property in target.GetKeys().Where(x => pattern.IsMatch(x)))
                    if (items[pattern].IsValid(definitions, target.Get(property)) == false)
                        return false;

            return true;
        }
    }
}