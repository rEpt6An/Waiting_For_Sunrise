using Newtonsoft.Json;
using System;

namespace Assets.C_.common
{
    public class JsonConverter
    {
        public static string ToJson(object obj)
        {
            return ToJson(obj, true, null);
        }

        public static string ToJson(object obj, bool indent = true, JsonSerializerSettings settings = null)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "要序列化的对象不能为null");
            }

            try
            {
                var formatting = indent ? Formatting.Indented : Formatting.None;

                if (settings != null)
                {
                    return JsonConvert.SerializeObject(obj, formatting, settings);
                }

                return JsonConvert.SerializeObject(obj, formatting);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("对象序列化为JSON时发生错误", nameof(obj), ex);
            }
        }

        // 快速转换（无缩进）
        public static string ToJsonCompact(object obj)
        {
            return ToJson(obj, false);
        }

        public static T FromJson<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException(nameof(json), "JSON字符串不能为空");
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("JSON反序列化时发生错误", nameof(json), ex);
            }
        }
    }
}