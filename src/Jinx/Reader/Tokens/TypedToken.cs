using System;

namespace Jinx.Reader.Tokens
{
    public class TypedToken : JsonToken
    {
        private readonly JsonTokenType type;

        public TypedToken(JsonTokenType type)
        {
            this.type = type;
        }

        public override JsonTokenType Type
        {
            get { return type; }
        }

        public override string GetString()
        {
            return String.Empty;
        }
    }
}