using Jinx.Dom;
using System;

namespace Jinx.Schema.Rules
{
    public class JsonMinimumRule : JsonSchemaRule
    {
        private readonly decimal minimum;

        public JsonMinimumRule(decimal minimum)
        {
            this.minimum = minimum;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            JsonNumber target = value as JsonNumber;

            if (target == null)
                return true;

            if (Decimal.Parse(target.Value) >= minimum)
                return true;

            return false;
        }
    }
}