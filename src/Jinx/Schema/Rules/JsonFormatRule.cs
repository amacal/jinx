using Jinx.Dom;
using System;
using System.Text.RegularExpressions;

namespace Jinx.Schema.Rules
{
    public class JsonFormatRule : JsonSchemaRule
    {
        private readonly string format;

        public JsonFormatRule(string format)
        {
            this.format = format;
        }

        public override bool IsValid(JsonSchemaDefinitions definitions, JsonValue value, JsonSchemaCallback callback)
        {
            switch (format)
            {
                case "uri":
                    return IsValidUri(value);

                case "ipv4":
                    return IsIpAddressV4(value);

                case "ipv6":
                    return IsIpAddressV6(value);

                case "date-time":
                case "email":

                default:
                    return true;
            }
        }

        private static bool IsValidUri(JsonValue value)
        {
            Uri uri;
            JsonText target = value as JsonText;

            if (target == null)
                return true;

            if (Uri.TryCreate(target.Value, UriKind.RelativeOrAbsolute, out uri))
                return true;

            return false;
        }

        private static bool IsIpAddressV4(JsonValue value)
        {
            string pattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            JsonText target = value as JsonText;

            if (target == null)
                return true;

            if (Regex.IsMatch(target.Value, pattern))
                return true;

            return false;
        }

        private static bool IsIpAddressV6(JsonValue value)
        {
            string pattern = @"(?:^|(?<=\s))(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))(?=\s|$)";
            JsonText target = value as JsonText;

            if (target == null)
                return true;

            if (Regex.IsMatch(target.Value, pattern))
                return true;

            return false;
        }
    }
}