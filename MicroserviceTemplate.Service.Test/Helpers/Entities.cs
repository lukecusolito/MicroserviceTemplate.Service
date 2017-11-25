using MicroserviceTemplate.Service.Models.Request;
using MicroserviceTemplate.Service.Utilities.Configuration;
using Newtonsoft.Json.Linq;

namespace MicroserviceTemplate.Service.Test.Helpers
{
    public class Entities
    {
        public static Settings GetSettingsMockData()
        {
            return new Settings
            {
                ApiPrefix = "/api",
                MicroserviceName = "TestService",
                RequestCorrelationIdIsRequired = false
            };
        }

        public static NancyRequest GenerateNancyRequest(string method = "", string url = "", JObject query = null, JObject body = null)
        {
            query = query != null ? query : new JObject();
            body = body != null ? body : new JObject();

            return new NancyRequest(method, url, query, body);
        }
    }
}
