using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MicroserviceTemplate.Service.Models.Request
{
    public class NancyRequest
    {
        public NancyRequest(string method, string url, dynamic query, JObject body)
        {
            Method = method;
            Url = url;
            Query = query;
            Body = body;
            Errors = new List<Error>();
        }

        public string Method { get; private set; }
        public string Url { get; private set; }
        public dynamic Query { get; private set; }
        public JObject Body { get; private set; }
        public List<Error> Errors { get; private set; }
    }
}