using Jinx.Dom;
using System;

namespace Jinx.Schema.Rules
{
    public class JsonEnumRule : JsonSchemaRule
    {
        private readonly JsonValue[] values;

        public JsonEnumRule(JsonValue[] values)
        {
            this.values = values;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            throw new NotImplementedException();
        }
    }
}