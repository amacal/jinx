using Jinx.Dom;
using System;

namespace Jinx.Schema
{
    public abstract class JsonSchemaRuleFactory
    {
        public abstract void Append(JsonSchemaRuleComponent rules, JsonObject definition, Func<JsonObject, JsonSchemaRule> parse);
    }
}