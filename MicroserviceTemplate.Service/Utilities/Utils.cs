using Nancy;
using Nancy.IO;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MicroserviceTemplate.Service.Utilities
{
    public static class Utils
    {
        public static JObject BodyToJObject(RequestStream body)
        {
            body.Position = 0;
            JObject jsonObject = null;
            using (var memory = new MemoryStream())
            {
                try
                {
                    body.CopyTo(memory);
                    var str = Encoding.UTF8.GetString(memory.ToArray());
                    if (!string.IsNullOrEmpty(str))
                        jsonObject = JObject.Parse(str);
                }
                catch { }
            }

            return jsonObject;
        }

        public static JObject ResponseToJObject(Response response)
        {
            JObject jsonObject;
            using (var memory = new MemoryStream())
            {
                response.Contents.Invoke(memory);
                var str = Encoding.UTF8.GetString(memory.ToArray());
                jsonObject = JObject.Parse(str);
            }

            return jsonObject;
        }

        public static JObject BodyXmlToJObject(RequestStream body)
        {
            body.Position = 0;
            JObject jsonObject = null;
            XmlSerializer serializer = new XmlSerializer(typeof(XmlDocument));
            try
            {
                var xmlDoc = serializer.Deserialize(body);
                jsonObject = JObject.FromObject(xmlDoc);
            }
            catch { }

            return jsonObject;
        }
    }
}