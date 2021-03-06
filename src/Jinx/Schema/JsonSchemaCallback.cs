﻿using Jinx.Dom;
using Jinx.Path;
using System.Collections.Generic;
using System.Linq;

namespace Jinx.Schema
{
    public class JsonSchemaCallback
    {
        public static JsonSchemaCallback Ignore()
        {
            return new JsonSchemaCallback();
        }

        private readonly ICollection<JsonSchemaMessage> items;
        private readonly JsonPath path;

        private JsonSchemaCallback()
        {
            this.path = JsonPath.Root;
        }

        private JsonSchemaCallback(JsonPath path, bool items)
        {
            this.path = path;
            this.items = items ? new List<JsonSchemaMessage>() : null;
        }

        public JsonSchemaCallback(ICollection<JsonSchemaMessage> items)
        {
            this.items = items;
            this.path = JsonPath.Root;
        }

        public int Count
        {
            get { return items?.Count ?? 0; }
        }

        public JsonSchemaCallback Scope()
        {
            return new JsonSchemaCallback(path, items != null);
        }

        public JsonSchemaCallback Scope(JsonPathSegment segment)
        {
            return new JsonSchemaCallback(path.Append(segment), items != null);
        }

        public bool Fail(JsonValue value, string description)
        {
            items?.Add(new JsonSchemaMessage(path, value, description));

            return false;
        }

        public bool Fail(JsonPathSegment segment, JsonValue value, string description)
        {
            items?.Add(new JsonSchemaMessage(path.Append(segment), value, description));

            return false;
        }

        public void Add(JsonSchemaMessage message)
        {
            items?.Add(message);
        }

        public void Add(JsonSchemaCallback scope)
        {
            if (scope.items != null && items != null)
                foreach (JsonSchemaMessage item in scope.items)
                    items.Add(item);
        }
    }
}