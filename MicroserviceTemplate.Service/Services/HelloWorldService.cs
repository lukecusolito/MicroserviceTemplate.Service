using MicroserviceTemplate.Service.Logging;
using MicroserviceTemplate.Service.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceTemplate.Service.Services
{
    public class HelloWorldService : IHelloWorldService
    {
        private static readonly NLogger Logger = new NLogger(typeof(HelloWorldService));
        public HelloWorldService()
        {

        }

        public virtual async Task<dynamic> HelloWorldPost()
        {
            throw new System.Exception("NO!");
            return new { Value = true };
        }

        public virtual async Task<dynamic> HelloWorld()
        {
            var x = Task.Factory.StartNew(() => getStringAsync());
            // Do some stuff
            var result = x.Result;

            return result;
        }

        private string getStringAsync()
        {
            Thread.Sleep(1000);
            return "Hello World!";
        }
    }
}