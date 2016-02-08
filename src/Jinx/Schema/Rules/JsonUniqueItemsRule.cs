using Jinx.Dom;
using System.Linq;

namespace Jinx.Schema.Rules
{
    public class JsonUniqueItemsRule : JsonSchemaRule
    {
        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            JsonArray target = value as JsonArray;

            if (target == null)
                return true;

            if (target.Items().Distinct().Count() != target.Count)
                return false;

            return true;
        }
    }
}