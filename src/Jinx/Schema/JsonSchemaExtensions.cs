using Jinx.Dom;

namespace Jinx.Schema
{
    public static class JsonSchemaExtensions
    {
        public static bool Call(this JsonSchemaCallback callback, string path, JsonValue value, string message)
        {
            callback.Invoke(path, value, message);
            return false;
        }
    }
}
