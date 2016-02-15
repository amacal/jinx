using Jinx.Reader.Exceptions;
using Jinx.Reader.Parsers;
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
            return ReadValue(JsonReaderState.Final);
        }

        private bool ReadPropertyOrEndObject()
        {
            switch (buffer.Data[buffer.Offset])
            {
                case '}':
                    return ReadEndObject();

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
                    return ReadEndObject();

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
                    return ReadEndArray();

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
                throw new JsonReaderSyntaxException();

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
                    return ReadStartObject(nextState);

                case '[':
                    return ReadStartArray(nextState);

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
            token = TextParser.Parse(buffer, textOrProperty);
            state = nextState;

            return true;
        }

        private bool ReadNumber(JsonReaderState nextState)
        {
            token = NumberParser.Parse(buffer);
            state = nextState;

            return true;
        }

        private bool ReadStartObject(JsonReaderState nextState)
        {
            stack.Push(nextState);
            state = JsonReaderState.BeginObject;
            token = new TypedToken(JsonTokenType.OpenObject);
            buffer.Forward(false);
            return true;
        }

        private bool ReadStartArray(JsonReaderState nextState)
        {
            stack.Push(nextState);
            state = JsonReaderState.BeginArray;
            token = new TypedToken(JsonTokenType.OpenArray);
            buffer.Forward(false);
            return true;
        }

        private bool ReadEndObject()
        {
            state = stack.Pop();
            token = new TypedToken(JsonTokenType.EndObject);
            buffer.Forward(false);
            return true;
        }

        private bool ReadEndArray()
        {
            state = stack.Pop();
            token = new TypedToken(JsonTokenType.EndArray);
            buffer.Forward(false);
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