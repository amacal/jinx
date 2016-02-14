using Jinx.Reader.Tokens;
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

        public bool HasError
        {
            get
            {
                return state == JsonReaderState.SyntaxError
                    || state == JsonReaderState.StreamError;
            }
        }

        public bool Next()
        {
            if (state == JsonReaderState.Final)
                return false;

            if (state == JsonReaderState.SyntaxError)
                return false;

            if (state == JsonReaderState.StreamError)
                return false;

            try
            {
                EnsureOrThrow(false);
                SkipWhiteSpaces();

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
                }
            }
            catch (JsonReaderStreamException)
            {
                state = JsonReaderState.StreamError;
            }
            catch (JsonReaderSyntaxException)
            {
                state = JsonReaderState.SyntaxError;
            }

            return false;
        }

        private bool ReadArrayOrObject()
        {
            if (ReadValue(JsonReaderState.Final) == false)
                throw new JsonReaderSyntaxException();

            return true;
        }

        private bool ReadPropertyOrEndObject()
        {
            switch (buffer.Data[buffer.Offset])
            {
                case '}':
                    state = stack.Pop();
                    token = new TypedToken(JsonTokenType.EndObject);
                    buffer.Forward(false);
                    return true;

                case '"':
                    return ReadTextOrProperty(JsonTokenType.Property, JsonReaderState.Property);
            }

            throw new JsonReaderSyntaxException();
        }

        private bool ReadValueOrEndArray()
        {
            switch (buffer.Data[buffer.Offset])
            {
                case ']':
                    state = stack.Pop();
                    token = new TypedToken(JsonTokenType.EndArray);
                    buffer.Forward(false);
                    return true;
            }

            return ReadValue(JsonReaderState.ValueInArray);
        }

        private bool ReadPropertySeparatorAndValue()
        {
            if (buffer.Data[buffer.Offset] != ':')
                throw new JsonReaderSyntaxException();

            buffer.Forward(false);
            SkipWhiteSpaces();

            return ReadValue(JsonReaderState.ValueInObject);
        }

        private bool ReadItemSeparatorAndPropertyOrEndObject()
        {
            switch (buffer.Data[buffer.Offset])
            {
                case '}':
                    state = stack.Pop();
                    token = new TypedToken(JsonTokenType.EndObject);
                    buffer.Forward(false);
                    return true;

                case ',':
                    return ReadItemSeparatorAndProperty();
            }

            throw new JsonReaderSyntaxException();
        }

        private bool ReadItemSeparatorAndValueOrEndArray()
        {
            switch (buffer.Data[buffer.Offset])
            {
                case ']':
                    state = stack.Pop();
                    token = new TypedToken(JsonTokenType.EndArray);
                    buffer.Forward(false);
                    return true;

                case ',':
                    return ReadItemSeparatorAndValue();
            }

            throw new JsonReaderSyntaxException();
        }

        private bool ReadItemSeparatorAndProperty()
        {
            ForwardOrThrow(false);
            SkipWhiteSpaces();

            if (buffer.Data[buffer.Offset] != '"')
                return false;

            return ReadTextOrProperty(JsonTokenType.Property, JsonReaderState.Property);
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
                    token = new TypedToken(JsonTokenType.OpenObject);
                    buffer.Forward(false);
                    return true;

                case '[':
                    stack.Push(nextState);
                    state = JsonReaderState.BeginArray;
                    token = new TypedToken(JsonTokenType.OpenArray);
                    buffer.Forward(false);
                    return true;

                case '"':
                    return ReadTextOrProperty(JsonTokenType.Text, nextState);

                case 't':
                    return ReadTrue(nextState);

                case 'f':
                    return ReadFalse(nextState);

                case 'n':
                    return ReadNull(nextState);
            }

            return ReadNumber(nextState);
        }

        private bool ReadTextOrProperty(JsonTokenType textOrProperty, JsonReaderState nextState)
        {
            bool escaped = false;
            int start = buffer.Offset + 1;
            int length = 0;

            ForwardOrThrow(true);

            while (buffer.Data[buffer.Offset] != '"')
            {
                if (buffer.Data[buffer.Offset] == '\\')
                {
                    escaped = true;
                    ForwardOrThrow(true);
                    length++;

                    char character = buffer.Data[buffer.Offset];

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
                            buffer.Forward(true);
                            length++;
                            continue;
                    }

                    if (character == 'u')
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            ForwardOrThrow(true);
                            length++;

                            character = buffer.Data[buffer.Offset];

                            if ('0' <= character && character <= '9')
                                continue;

                            if ('a' <= character && character <= 'f')
                                continue;

                            throw new JsonReaderSyntaxException();
                        }

                        ForwardOrThrow(true);
                        length++;
                        continue;
                    }

                    throw new JsonReaderSyntaxException();
                }

                ForwardOrThrow(true);
                length++;
            }

            buffer.Forward(true);
            state = nextState;

            if (escaped)
                token = new EscapingToken(textOrProperty, buffer.Data, start, length);
            else
                token = new DataToken(textOrProperty, buffer.Data, start, length);

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
                throw new JsonReaderSyntaxException();

            state = nextState;
            token = new DataToken(JsonTokenType.Number, buffer.Data, start, length);
            return true;
        }

        private bool ReadTrue(JsonReaderState nextState)
        {
            if (Skip("true"))
            {
                state = nextState;
                token = new TypedToken(JsonTokenType.True);
                return true;
            }

            throw new JsonReaderSyntaxException();
        }

        private bool ReadFalse(JsonReaderState nextState)
        {
            if (Skip("false"))
            {
                state = nextState;
                token = new TypedToken(JsonTokenType.False);
                return true;
            }

            throw new JsonReaderSyntaxException();
        }

        private bool ReadNull(JsonReaderState nextState)
        {
            if (Skip("null"))
            {
                state = nextState;
                token = new TypedToken(JsonTokenType.Null);
                return true;
            }

            throw new JsonReaderSyntaxException();
        }

        private void SkipWhiteSpaces()
        {
            if (buffer.Ensure(false))
                while (Char.IsWhiteSpace(buffer.Data[buffer.Offset]))
                    ForwardOrThrow(false);

            if (buffer.Ensure(false))
                return;

            throw new JsonReaderStreamException();
        }

        private bool Skip(string data)
        {
            foreach (char character in data)
            {
                if (buffer.Ensure(false) == false)
                    return false;

                if (buffer.Data[buffer.Offset] != character)
                    return false;

                buffer.Forward(false);
            }

            return true;
        }

        private void EnsureOrThrow(bool consistent)
        {
            if (buffer.Ensure(consistent) == false)
                throw new JsonReaderStreamException();
        }

        private void ForwardOrThrow(bool consistent)
        {
            if (buffer.Forward(consistent) == false)
                throw new JsonReaderStreamException();
        }
    }
}