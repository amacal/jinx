using System;
using System.Globalization;

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
            get { return value; }
        }

        public bool IsInteger()
        {
            return value.Contains(".") == false;
        }

        public int ToInt32()
        {
            return Int32.Parse(value, CultureInfo.InvariantCulture);
        }

        public decimal ToDecimal()
        {
            return Decimal.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            JsonNumber other = obj as JsonNumber;

            return other != null
                && other.value == Value;
        }
    }
}