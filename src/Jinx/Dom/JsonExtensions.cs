namespace Jinx.Dom
{
    public static class JsonExtensions
    {
        public static T As<T>(this JsonValue value)
            where T : JsonValue
        {
            return value as T;
        }
    }
}
