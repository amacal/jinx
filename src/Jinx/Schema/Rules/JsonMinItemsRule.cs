using Jinx.Dom;

namespace Jinx.Schema.Rules
{
    public class JsonMinItemsRule : JsonSchemaRule
    {
        private readonly int threashold;

        public JsonMinItemsRule(int threashold)
        {
            this.threashold = threashold;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            JsonArray target = value as JsonArray;

            if (target == null)
                return true;

            if (threashold > target.Count)
                return false;

            return true;
        }
    }
}