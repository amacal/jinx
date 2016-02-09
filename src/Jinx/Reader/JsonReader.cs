using System;
using System.Collections.Generic;
using System.IO;

namespace Jinx.Reader
{
    public class JsonReader
    {
        private Stack<JsonReaderState> stack;

        private JsonReaderBuffer buffer;
        private JsonReaderState state;
        private JsonToken token;

        public JsonReader(TextReader stream)
        {
            this.state = JsonReaderState.Start;
            this.stack = new Stack<JsonReaderState>();

            this.buffer = new JsonReaderBuffer
            {
                Size = 1024,
                Stream = stream,
                Data = new char[1024]
            };
        }

        public JsonToken Token
        {
            get { return token; }
        }

        public bool Next()
        {
            switch (state)
            {
                case JsonReaderState.Start:
                    return ReadArrayOrObject();

                case JsonReaderState.BeginObject:
                    return ReadPropertyOrEndObject();

                case JsonReaderState.BeginArray:
                    return ReadValueOrEndArray();

                case JsonReaderState.Property:
                    return ReadPropertySeparatorAndValue();

                case JsonReaderState.ValueInObject:
                    return ReadItemSeparatorAndPropertyOrEndObject();

                case JsonReaderState.ValueInArray:
                    return ReadItemSeparatorAndValueOrEndArray();

                case JsonReaderState.SyntaxError:
                    return false;

                case JsonReaderState.StreamError:
                    return false;
            }

            return false;
        }

        private bool ReadArrayOrObject()
        {
            buffer.Ensure(false);
            SkipWhiteSpaces();

            if (ReadValue(JsonReaderState.Final) == false)
            {
                state = JsonReaderState.SyntaxError;
                return false;
            }

            return true;
        }

        private bool ReadPropertyOrEndObject()
        {
            buffer.Ensure(false);
            SkipWhiteSpaces();

            switch (buffer.Data[buffer.Offset])
            {
                case '}':
                    state = stack.Pop();
                    token = new JsonToken(JsonTokenType.EndObject);
                    buffer.Forward(false);
                    return true;

                case '"':
                    return ReadProperty();
            }

            state = JsonReaderState.SyntaxError;
            return false;
        }

        private bool ReadValueOrEndArray()
        {
            buffer.Ensure(false);
            SkipWhiteSpaces();

            switch (buffer.Data[buffer.Offset])
            {
                case ']':
                    state = stack.Pop();
                    token = new JsonToken(JsonTokenType.EndArray);
                    buffer.Forward(false);
                    return true;
            }

            return ReadValue(JsonReaderState.ValueInArray);
        }

        private bool ReadPropertySeparatorAndValue()
        {
            buffer.Ensure(false);
            SkipWhiteSpaces();

            if (buffer.Data[buffer.Offset] != ':')
            {
                state = JsonReaderState.SyntaxError;
                return false;
            }

            buffer.Forward(false);
            SkipWhiteSpaces();

            return ReadValue(JsonReaderState.ValueInObject);
        }

        private bool ReadItemSeparatorAndPropertyOrEndObject()
        {
            buffer.Ensure(false);
            SkipWhiteSpaces();

            switch (buffer.Data[buffer.Offset])
            {
                case '}':
                    state = stack.Pop();
                    token = new JsonToken(JsonTokenType.EndObject);
                    buffer.Forward(false);
                    return true;

                case ',':
                    return ReadItemSeparatorAndProperty();
            }

            return false;
        }

        private bool ReadItemSeparatorAndValueOrEndArray()
        {
            buffer.Ensure(false);
            SkipWhiteSpaces();

            switch (buffer.Data[buffer.Offset])
            {
                case ']':
                    state = stack.Pop();
                    token = new JsonToken(JsonTokenType.EndArray);
                    buffer.Forward(false);
                    return true;

                case ',':
                    return ReadItemSeparatorAndValue();
            }

            return false;
        }

        private bool ReadItemSeparatorAndProperty()
        {
            buffer.Forward(false);
            SkipWhiteSpaces();

            if (buffer.Data[buffer.Offset] != '"')
                return false;

            return ReadProperty();
        }

        private bool ReadItemSeparatorAndValue()
        {
            buffer.Forward(false);
            SkipWhiteSpaces();

            return ReadValue(JsonReaderState.ValueInArray);
        }

        private bool ReadValue(JsonReaderState nextState)
        {
            switch (buffer.Data[buffer.Offset])
            {
                case '{':
                    stack.Push(nextState);
                    state = JsonReaderState.BeginObject;
                    token = new JsonToken(JsonTokenType.OpenObject);
                    buffer.Forward(false);
                    return true;

                case '[':
                    stack.Push(nextState);
                    state = JsonReaderState.BeginArray;
                    token = new JsonToken(JsonTokenType.OpenArray);
                    buffer.Forward(false);
                    return true;

                case '"':
                    return ReadText(nextState);

                case 't':
                    return ReadTrue(nextState);

                case 'f':
                    return ReadFalse(nextState);

                case 'n':
                    return ReadNull(nextState);
            }

            return ReadNumber(nextState);
        }

        private bool ReadProperty()
        {
            int start = buffer.Offset + 1;
            int length = 0;

            buffer.Forward(true);

            while (buffer.Data[buffer.Offset] != '"')
            {
                buffer.Forward(true);
                length++;
            }

            buffer.Forward(true);

            state = JsonReaderState.Property;
            token = new JsonToken(JsonTokenType.Property, buffer.Data, start, length, false);
            return true;
        }

        private bool ReadText(JsonReaderState nextState)
        {
            bool escaped = false;
            int start = buffer.Offset + 1;
            int length = 0;

            buffer.Forward(true);

            while (buffer.Data[buffer.Offset] != '"')
            {
                if (buffer.Data[buffer.Offset] == '\\')
                {
                    escaped = true;
                    buffer.Forward(true);
                    length++;
                }

                buffer.Forward(true);
                length++;
            }

            buffer.Forward(true);

            state = nextState;
            token = new JsonToken(JsonTokenType.Text, buffer.Data, start, length, escaped);
            return true;
        }

        private bool ReadNumber(JsonReaderState nextState)
        {
            int start = buffer.Offset;
            int length = 0;

            Func<char, bool> isAcceptable = character =>
            {
                return Char.IsDigit(character) || character == '.' || character == '-' || character == 'e';
            };

            while (isAcceptable.Invoke(buffer.Data[buffer.Offset]))
            {
                buffer.Forward(true);
                length++;
            }

            if (length == 0)
                return false;

            state = nextState;
            token = new JsonToken(JsonTokenType.Number, buffer.Data, start, length, false);
            return true;
        }

        private bool ReadTrue(JsonReaderState nextState)
        {
            if (Skip("true"))
            {
                state = nextState;
                token = new JsonToken(JsonTokenType.True);
                return true;
            }

            state = JsonReaderState.SyntaxError;
            return false;
        }

        private bool ReadFalse(JsonReaderState nextState)
        {
            if (Skip("false"))
            {
                state = nextState;
                token = new JsonToken(JsonTokenType.False);
                return true;
            }

            state = JsonReaderState.SyntaxError;
            return false;
        }

        private bool ReadNull(JsonReaderState nextState)
        {
            if (Skip("null"))
            {
                state = nextState;
                token = new JsonToken(JsonTokenType.Null);
                return true;
            }

            state = JsonReaderState.SyntaxError;
            return false;
        }

        private void SkipWhiteSpaces()
        {
            while (Char.IsWhiteSpace(buffer.Data[buffer.Offset]))
            {
                buffer.Forward(false);
            }
        }

        private bool Skip(string data)
        {
            foreach (char character in data)
            {
                if (buffer.Data[buffer.Offset] != character)
                    return false;

                buffer.Forward(false);
            }

            return true;
        }
    }
}