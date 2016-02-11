namespace Jinx.Dom
{
    public class JsonTrue : JsonValue
    {
        public override int GetHashCode()
        {
            return 1;
        }

        public override bool Equals(object obj)
        {
            return obj is JsonTrue;
        }
    }
}