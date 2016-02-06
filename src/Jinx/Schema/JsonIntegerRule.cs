﻿using Jinx.Dom;

namespace Jinx.Schema
{
    public class JsonIntegerRule : JsonSchemaRule
    {
        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue number)
        {
            JsonNumber text = number as JsonNumber;

            if (text == null)
                return false;

            return true;
        }
    }
}