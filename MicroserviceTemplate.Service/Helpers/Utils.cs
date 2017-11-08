using Nancy;
using Nancy.IO;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceTemplate.Service.Helpers
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
    }
}