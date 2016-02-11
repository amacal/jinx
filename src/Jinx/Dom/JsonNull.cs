namespace Jinx.Dom
{
    public class JsonNull : JsonValue
    {
        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            return obj is JsonNull;
        }
    }
}