namespace Jinx.Reader.Tokens
{
    public class DataToken : JsonToken
    {
        private readonly JsonTokenType type;
        private readonly char[] data;
        private readonly int offset;
        private readonly int length;

        public DataToken(JsonTokenType type, char[] data, int offset, int length)
        {
            this.type = type;
            this.data = data;
            this.offset = offset;
            this.length = length;
        }

        public override JsonTokenType Type
        {
            get { return type; }
        }

        public override string GetString()
        {
            return new string(data, offset, length);
        }
    }
}