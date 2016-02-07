using Jinx.Dom;
using System;

namespace Jinx.Schema.Rules
{
    public class JsonFormatRule : JsonSchemaRule
    {
        private readonly string format;

        public JsonFormatRule(string format)
        {
            this.format = format;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            throw new NotImplementedException();
        }
    }
}