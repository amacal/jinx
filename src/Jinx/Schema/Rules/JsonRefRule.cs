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
            List<string> messages = new List<string>();

            if (definitions.Resolve(reference).IsValid(value, messages))
                return true;

            foreach (string message in messages)
                callback.Call("", value, message);

            return callback.Call("", value, $"Referenced schema '{reference}' is invalid.");
        }
    }
}