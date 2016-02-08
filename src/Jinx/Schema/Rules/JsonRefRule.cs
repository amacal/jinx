using Jinx.Dom;

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
            return definitions.Resolve(reference).IsValid(value);
        }
    }
}