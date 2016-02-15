using Jinx.Reader.Exceptions;
using Jinx.Reader.Tokens;
using System;

namespace Jinx.Reader.Parsers
{
    public static class NumberParser
    {
        public static JsonToken Parse(JsonReaderBuffer buffer)
        {
            int start = buffer.Offset;
            int length = 0;

            Func<char, bool> isAcceptable = character =>
            {
                return Char.IsDigit(character) || character == '.' || character == '-' || character == '+' || character == 'e';
            };

            while (isAcceptable.Invoke(buffer.Data[buffer.Offset]))
            {
                buffer.Forward(true);
                length++;
            }

            if (length == 0)
                throw new JsonReaderSyntaxException();

            return new DataToken(JsonTokenType.Number, buffer.Data, start, length);
        }
    }
}