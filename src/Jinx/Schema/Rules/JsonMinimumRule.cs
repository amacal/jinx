using Jinx.Dom;
using System;

namespace Jinx.Schema.Rules
{
    public class JsonMinimumRule : JsonSchemaRule
    {
        private readonly decimal threashold;

        public JsonMinimumRule(decimal threashold)
        {
            this.threashold = threashold;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            JsonNumber target = value as JsonNumber;

            if (target == null)
                return true;

            if (threashold > Decimal.Parse(target.Value))
                return false;

            return true;
        }
    }
}