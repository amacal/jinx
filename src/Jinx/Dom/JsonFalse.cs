namespace Jinx.Dom
{
    public class JsonFalse : JsonValue
    {
        public override int GetHashCode()
        {
            return -1;
        }

        public override bool Equals(object obj)
        {
            return obj is JsonFalse;
        }

        public override string ToString()
        {
            return "false";
        }
    }
}