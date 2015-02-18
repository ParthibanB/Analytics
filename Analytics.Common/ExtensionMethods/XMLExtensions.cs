using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Common.ExtensionMethods
{
    public class XmlExtensions
    {
        public static string Serialize<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, obj);
                return builder.ToString();
            }
        }

        public static T Deserialize<T>(string content)
        {
            if (String.IsNullOrEmpty(content))
            {
                return default(T);
            }
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                T result = default(T);
                using (var reader = new StringReader(content))
                {
                    result = (T)xmlSerializer.Deserialize(reader);
                }

                return result;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
