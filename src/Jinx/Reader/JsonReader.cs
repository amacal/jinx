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
                Size = 20480,
                Stream = stream,
                Data = new char[20480]
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
            buffer.Ensure();
            SkipWhiteSpaces();

            switch (buffer.Data[buffer.Offset])
            {
                case '{':
                    stack.Push(JsonReaderState.Final);
                    state = JsonReaderState.BeginObject;
                    token = new JsonToken(JsonTokenType.OpenObject, buffer.Data, buffer.Offset, 1);
                    buffer.Forward();
                    return true;

                case '[':
                    stack.Push(JsonReaderState.Final);
                    state = JsonReaderState.BeginArray;
                    token = new JsonToken(JsonTokenType.OpenArray, buffer.Data, buffer.Offset, 1);
                    buffer.Forward();
                    return true;
            }

            state = JsonReaderState.SyntaxError;
            return false;
        }

        private bool ReadPropertyOrEndObject()
        {
            buffer.Ensure();
            SkipWhiteSpaces();

            switch (buffer.Data[buffer.Offset])
            {
                case '}':
                    state = stack.Pop();
                    token = new JsonToken(JsonTokenType.EndObject, buffer.Data, buffer.Offset, 1);
                    buffer.Forward();
                    return true;

                case '"':
                    return ReadProperty();
            }

            state = JsonReaderState.SyntaxError;
            return false;
        }

        private bool ReadValueOrEndArray()
        {
            buffer.Ensure();
            SkipWhiteSpaces();

            switch (buffer.Data[buffer.Offset])
            {
                case ']':
                    state = stack.Pop();
                    token = new JsonToken(JsonTokenType.EndArray, buffer.Data, buffer.Offset, 1);
                    buffer.Forward();
                    return true;
            }

            return ReadValue(JsonReaderState.ValueInArray);
        }

        private bool ReadPropertySeparatorAndValue()
        {
            buffer.Ensure();
            SkipWhiteSpaces();

            if (buffer.Data[buffer.Offset] != ':')
            {
                state = JsonReaderState.SyntaxError;
                return false;
            }

            buffer.Forward();
            SkipWhiteSpaces();

            return ReadValue(JsonReaderState.ValueInObject);
        }

        private bool ReadItemSeparatorAndPropertyOrEndObject()
        {
            buffer.Ensure();
            SkipWhiteSpaces();

            switch (buffer.Data[buffer.Offset])
            {
                case '}':
                    state = stack.Pop();
                    token = new JsonToken(JsonTokenType.EndObject, buffer.Data, buffer.Offset, 1);
                    buffer.Forward();
                    return true;

                case ',':
                    return ReadItemSeparatorAndProperty();
            }

            return false;
        }

        private bool ReadItemSeparatorAndValueOrEndArray()
        {
            buffer.Ensure();
            SkipWhiteSpaces();

            switch (buffer.Data[buffer.Offset])
            {
                case ']':
                    state = stack.Pop();
                    token = new JsonToken(JsonTokenType.EndArray, buffer.Data, buffer.Offset, 1);
                    buffer.Forward();
                    return true;

                case ',':
                    return ReadItemSeparatorAndValue();
            }

            return false;
        }

        private bool ReadItemSeparatorAndProperty()
        {
            buffer.Forward();
            SkipWhiteSpaces();

            if (buffer.Data[buffer.Offset] != '"')
                return false;

            return ReadProperty();
        }

        private bool ReadItemSeparatorAndValue()
        {
            buffer.Forward();
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
                    token = new JsonToken(JsonTokenType.OpenObject, buffer.Data, buffer.Offset, 1);
                    buffer.Forward();
                    return true;

                case '[':
                    stack.Push(nextState);
                    state = JsonReaderState.BeginArray;
                    token = new JsonToken(JsonTokenType.OpenArray, buffer.Data, buffer.Offset, 1);
                    buffer.Forward();
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

            buffer.Forward();

            while (buffer.Data[buffer.Offset] != '"')
            {
                buffer.Forward();
                length++;
            }

            buffer.Forward();

            state = JsonReaderState.Property;
            token = new JsonToken(JsonTokenType.Property, buffer.Data, start, length);
            return true;
        }

        private bool ReadText(JsonReaderState nextState)
        {
            int start = buffer.Offset + 1;
            int length = 0;

            buffer.Forward();

            while (buffer.Data[buffer.Offset] != '"')
            {
                buffer.Forward();
                length++;
            }

            buffer.Forward();

            state = nextState;
            token = new JsonToken(JsonTokenType.Text, buffer.Data, start, length);
            return true;
        }

        private bool ReadNumber(JsonReaderState nextState)
        {
            int start = buffer.Offset;
            int length = 0;

            Func<char, bool> isAcceptable = character =>
            {
                return Char.IsDigit(character) || character == '.';
            };

            while (isAcceptable.Invoke(buffer.Data[buffer.Offset]))
            {
                buffer.Forward();
                length++;
            }

            if (length == 0)
                return false;

            state = nextState;
            token = new JsonToken(JsonTokenType.Number, buffer.Data, start, length);
            return true;
        }

        private bool ReadTrue(JsonReaderState nextState)
        {
            if (Skip("true"))
            {
                state = nextState;
                token = new JsonToken(JsonTokenType.True, buffer.Data, buffer.Offset - 4, 4);
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
                token = new JsonToken(JsonTokenType.False, buffer.Data, buffer.Offset - 5, 5);
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
                token = new JsonToken(JsonTokenType.Null, buffer.Data, buffer.Offset - 4, 4);
                return true;
            }

            state = JsonReaderState.SyntaxError;
            return false;
        }

        private void SkipWhiteSpaces()
        {
            while (Char.IsWhiteSpace(buffer.Data[buffer.Offset]))
            {
                buffer.Forward();
            }
        }

        private bool Skip(string data)
        {
            foreach (char character in data)
            {
                if (buffer.Data[buffer.Offset] != character)
                    return false;

                buffer.Forward();
            }

            return true;
        }
    }
}