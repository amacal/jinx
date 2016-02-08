using Jinx.Dom;

namespace Jinx.Schema
{
    public abstract class JsonSchemaRule
    {
        public abstract bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback);
    }
}