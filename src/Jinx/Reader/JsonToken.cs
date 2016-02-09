namespace Jinx.Reader
{
    public class JsonToken
    {
        private readonly JsonTokenType type;
        private readonly char[] data;
        private readonly int offset;
        private readonly int length;
        private readonly bool escaped;

        public JsonToken(JsonTokenType type)
        {
            this.type = type;
        }

        public JsonToken(JsonTokenType type, char[] data, int offset, int length, bool escaped)
        {
            this.type = type;
            this.data = data;
            this.offset = offset;
            this.length = length;
            this.escaped = escaped;
        }

        public JsonTokenType Type
        {
            get { return type; }
        }

        public string GetString()
        {
            string result = new string(data, offset, length);

            if (escaped)
            {
                result = result.Replace(@"\\", @"\");
            }

            return result;
        }
    }
}