using Jinx.Dom;
using System;

namespace Jinx.Schema.Rules
{
    public class JsonUniqueItemsRule : JsonSchemaRule
    {
        private readonly bool isUnique;

        public JsonUniqueItemsRule(bool isUnique)
        {
            this.isUnique = isUnique;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value)
        {
            throw new NotImplementedException();
        }
    }
}