using Jinx.Reader.Exceptions;
using Jinx.Reader.Tokens;
using System;

namespace Jinx.Reader.Parsers
{
    public static class NumberParser
    {
        public static JsonToken Parse(JsonReaderBuffer buffer)
        {
            bool isReal = false;
            bool isNegative = false;
            bool isScientific = false;

            int start = buffer.Offset;
            int length = 0;

            Action forward = () =>
            {
                ForwardOrThrow(buffer, true);
                length++;
            };

            Func<bool> tryForward = () =>
            {
                length++;
                return buffer.Forward(true);
            };

            if (buffer.Data[buffer.Offset] == '-')
            {
                isNegative = true;
                forward();
            }

            if (buffer.Data[buffer.Offset] == '0')
            {
                tryForward();
            }
            else if (Char.IsDigit(buffer.Data[buffer.Offset]))
            {
                while (Char.IsDigit(buffer.Data[buffer.Offset]))
                    if (tryForward() == false)
                        break;
            }
            else
                throw new JsonReaderSyntaxException();

            if (buffer.Ensure(true))
            {
                if (buffer.Data[buffer.Offset] == '.')
                {
                    forward();
                    isReal = true;

                    if (Char.IsDigit(buffer.Data[buffer.Offset]) == false)
                        throw new JsonReaderSyntaxException();

                    while (Char.IsDigit(buffer.Data[buffer.Offset]))
                        if (tryForward() == false)
                            break;
                }

                if (buffer.Ensure(true))
                {
                    if (buffer.Data[buffer.Offset] == 'e' ||
                        buffer.Data[buffer.Offset] == 'E')
                    {
                        forward();
                        isScientific = true;

                        if (buffer.Data[buffer.Offset] == '-' ||
                            buffer.Data[buffer.Offset] == '+')
                        {
                            forward();
                        }

                        while (Char.IsDigit(buffer.Data[buffer.Offset]))
                            if (tryForward() == false)
                                break;
                    }
                }
            }

            if (length == 0)
                throw new JsonReaderSyntaxException();

            return new DataToken(JsonTokenType.Number, buffer.Data, start, length);
        }

        private static void ForwardOrThrow(JsonReaderBuffer buffer, bool consistent)
        {
            if (buffer.Forward(consistent) == false)
                throw new JsonReaderStreamException();
        }
    }
}