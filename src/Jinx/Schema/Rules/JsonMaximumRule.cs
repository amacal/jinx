using Jinx.Dom;
using System;

namespace Jinx.Schema.Rules
{
    public class JsonMaximumRule : JsonSchemaRule
    {
        private readonly decimal maximum;

        public JsonMaximumRule(decimal maximum)
        {
            this.maximum = maximum;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            JsonNumber target = value as JsonNumber;

            if (target == null)
                return true;

            if (Decimal.Parse(target.Value) <= maximum)
                return true;

            return false;
        }
    }
}