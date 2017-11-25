using Newtonsoft.Json;

namespace MicroserviceTemplate.Service.Utilities
{
    public class JsonSerializer
    {
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}