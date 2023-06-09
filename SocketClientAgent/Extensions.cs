using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientAgent
{
    public static class Extensions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T? ToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static bool TryGetValue<T>(this JObject obj, string key, out T? value, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            value = default;

            var keys = key.Split('.', StringSplitOptions.TrimEntries);

            var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            var token = obj.GetValue(keys[0], comparison);
            for (int i = 1; i < keys.Length; i++)
            {
                if (token is JObject o)
                {
                    token = o.GetValue(keys[i], comparison);
                }
                else
                {
                    token = null;
                    break;
                }
            }

            if (token != null)
            {
                value = token.Value<T>();
                return true;
            }
            return false;
        }
    }
}
