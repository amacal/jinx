using System;

namespace Jinx.Schema
{
    public class JsonSchemaPath
    {
        public static readonly JsonSchemaPath Root;

        static JsonSchemaPath()
        {
            Root = new JsonSchemaPath();
        }

        private readonly string path;

        private JsonSchemaPath()
        {
            this.path = String.Empty;
        }

        private JsonSchemaPath(string path)
        {
            this.path = path;
        }

        public JsonSchemaPath Drill(string name)
        {
            return new JsonSchemaPath(path + name);
        }
    }
}