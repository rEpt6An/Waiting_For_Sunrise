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
                throw new ArgumentNullException(nameof(obj), "Ҫ���л��Ķ�����Ϊnull");
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
                throw new ArgumentException("�������л�ΪJSONʱ��������", nameof(obj), ex);
            }
        }

        // ����ת������������
        public static string ToJsonCompact(object obj)
        {
            return ToJson(obj, false);
        }

        public static T FromJson<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException(nameof(json), "JSON�ַ�������Ϊ��");
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("JSON�����л�ʱ��������", nameof(json), ex);
            }
        }
    }
}