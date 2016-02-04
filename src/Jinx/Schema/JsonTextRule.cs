using Jinx.Dom;

namespace Jinx.Schema
{
    public class JsonTextRule : JsonSchemaRule
    {
        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            JsonText text = value as JsonText;

            if (text == null)
                return false;

            return true;
        }
    }
}
