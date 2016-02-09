using Jinx.Dom;
using System.Collections.Generic;

namespace Jinx.Schema.Rules
{
    public class JsonTypeRule : JsonSchemaRule
    {
        private readonly List<string> types;

        public JsonTypeRule()
        {
            this.types = new List<string>();
        }

        public void AddType(string type)
        {
            types.Add(type);
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            foreach (string type in types)
                if (HasType(value, type))
                    return true;

            return false;
        }

        private static bool HasType(JsonValue value, string type)
        {
            switch (type)
            {
                case "object":
                    return value is JsonObject;

                case "array":
                    return value is JsonArray;

                case "string":
                    return value is JsonText;

                case "boolean":
                    return value is JsonTrue || value is JsonFalse;

                case "null":
                    return value is JsonNull;

                case "integer":
                    return value is JsonNumber && value.As<JsonNumber>().IsInteger();

                case "number":
                    return value is JsonNumber;

                default:
                    return false;
            }
        }
    }
}