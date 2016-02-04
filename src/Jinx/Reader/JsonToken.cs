namespace Jinx.Reader
{
    public class JsonToken
    {
        private readonly JsonTokenType type;
        private readonly char[] data;
        private readonly int offset;
        private readonly int length;

        public JsonToken(JsonTokenType type, char[] data, int offset, int length)
        {
            this.type = type;
            this.data = data;
            this.offset = offset;
            this.length = length;
        }

        public JsonTokenType Type
        {
            get { return type; }
        }

        public string GetString()
        {
            return new string(data, offset, length);
        }
    }
}