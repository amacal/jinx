using Jinx.Dom;
using System.Text.RegularExpressions;

namespace Jinx.Schema.Rules
{
    public class JsonPatternRule : JsonSchemaRule
    {
        private readonly Regex pattern;

        public JsonPatternRule(string pattern)
        {
            this.pattern = new Regex(pattern);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            JsonText target = value as JsonText;

            if (target == null)
                return true;

            if (pattern.IsMatch(target.Value))
                return true;

            return false;
        }
    }
}