using MicroserviceTemplate.Service.Services.Interfaces;
using Nancy;

namespace MicroserviceTemplate.Service
{
    public class HelloWorldModule : NancyModule
    {
        readonly IHelloWorldService _helloWorldService;
        public HelloWorldModule(IHelloWorldService helloWorldService)
        {
            _helloWorldService = helloWorldService;
            //Before += ctx =>
            //{
            //    // Validate schema?
            //    return null;
            //};

            //After += ctx =>
            //{

            //};

            Get["/", true] = async (x, ct) => await _helloWorldService.HelloWorld();
            Get["/api/value", true] = async (x, ct) => await _helloWorldService.HelloWorldPost();
        }
    }
}