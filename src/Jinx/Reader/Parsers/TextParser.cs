using Jinx.Reader.Exceptions;
using Jinx.Reader.Tokens;
using System;

namespace Jinx.Reader.Parsers
{
    public static class TextParser
    {
        public static JsonToken Parse(JsonReaderBuffer buffer, JsonTokenType textOrProperty)
        {
            char character;
            bool escaped = false;

            int start = buffer.Offset + 1;
            int length = 0;

            Action forward = () =>
            {
                ForwardOrThrow(buffer, true);
                length++;
            };

            ForwardOrThrow(buffer, true);

            while (buffer.Data[buffer.Offset] != '"')
            {
                if (buffer.Data[buffer.Offset] != '\\')
                {
                    forward();
                    continue;
                }

                escaped = true;
                forward();

                character = buffer.Data[buffer.Offset];

                switch (character)
                {
                    case 'n':
                    case 't':
                    case 'r':
                    case 'b':
                    case 'f':
                    case '/':
                    case '\\':
                    case '\"':
                        forward();
                        continue;
                }

                if (character != 'u')
                    throw new JsonReaderSyntaxException();

                for (int i = 0; i < 4; i++)
                {
                    forward();
                    character = buffer.Data[buffer.Offset];

                    if ('0' <= character && character <= '9')
                        continue;

                    if ('a' <= character && character <= 'f')
                        continue;

                    throw new JsonReaderSyntaxException();
                }

                forward();
                continue;
            }

            buffer.Forward(true);

            if (escaped)
                return new EscapingToken(textOrProperty, buffer.Data, start, length);

            return new DataToken(textOrProperty, buffer.Data, start, length);
        }

        private static void ForwardOrThrow(JsonReaderBuffer buffer, bool consistent)
        {
            if (buffer.Forward(consistent) == false)
                throw new JsonReaderStreamException();
        }
    }
}