using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonRefRule : JsonSchemaRule
    {
        private readonly string reference;

        public JsonRefRule(string reference)
        {
            this.reference = reference;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            List<JsonSchemaMessage> messages = new List<JsonSchemaMessage>();

            if (definitions.Resolve(reference).IsValid(value, messages))
                return true;

            foreach (JsonSchemaMessage message in messages)
                callback.Add(message);

            return callback.Call(value, $"Referenced schema '{reference}' is invalid.");
        }
    }
}