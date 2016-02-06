using Jinx.Dom;

namespace Jinx.Schema
{
    public class JsonNoRule : JsonSchemaRule
    {
        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            return true;
        }
    }
}