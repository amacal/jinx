namespace Jinx.Dom
{
    public class JsonText : JsonValue
    {
        private readonly string value;

        public JsonText(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            JsonText other = obj as JsonText;

            return other != null
                && other.Value == Value;
        }

        public override string ToString()
        {
            return $@"""{value}""";
        }
    }
}