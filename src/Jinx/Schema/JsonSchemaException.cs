using System;

namespace Jinx.Schema
{
    [Serializable]
    public class JsonSchemaException : Exception
    {
        public JsonSchemaException(string message)
            : base(message)
        {
        }
    }
}