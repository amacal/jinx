using Jinx.Dom;

namespace Jinx.Schema
{
    public class JsonReferenceRule : JsonSchemaRule
    {
        private readonly string reference;

        public JsonReferenceRule(string reference)
        {
            this.reference = reference;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            return definitions.Resolve(reference).IsValid(value);
        }
    }
}
