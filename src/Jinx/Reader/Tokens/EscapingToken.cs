using System;
using System.Globalization;
using System.Text;

namespace Jinx.Reader.Tokens
{
    public class EscapingToken : JsonToken
    {
        private readonly JsonTokenType type;
        private readonly char[] data;
        private readonly int offset;
        private readonly int length;

        public EscapingToken(JsonTokenType type, char[] data, int offset, int length)
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
            StringBuilder builder = new StringBuilder();

            for (int i = offset; i < offset + length; i++)
            {
                if (data[i] == '\\')
                {
                    Escape(builder, ref i);
                }
                else
                {
                    builder.Append(data[i]);
                }
            }

            return builder.ToString();
        }

        private void Escape(StringBuilder builder, ref int position)
        {
            switch (data[position + 1])
            {
                case 'r':
                    builder.Append("\r");
                    position++;
                    break;

                case 'n':
                    builder.Append("\n");
                    position++;
                    break;

                case 'b':
                    builder.Append("\b");
                    position++;
                    break;

                case 't':
                    builder.Append("\t");
                    position++;
                    break;

                case 'f':
                    builder.Append("\f");
                    position++;
                    break;

                case '"':
                    builder.Append("\"");
                    position++;
                    break;

                case '/':
                    builder.Append("/");
                    position++;
                    break;

                case '\\':
                    builder.Append("\\");
                    position++;
                    break;

                case 'u':
                    builder.Append(Char.ConvertFromUtf32(Int32.Parse(new string(data, position + 2, 4), NumberStyles.HexNumber)));
                    position += 5;
                    break;
            }
        }
    }
}
