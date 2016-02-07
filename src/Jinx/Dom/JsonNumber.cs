namespace Jinx.Dom
{
    public class JsonNumber : JsonValue
    {
        private readonly string value;

        public JsonNumber(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return this.value; }
        }
    }
}