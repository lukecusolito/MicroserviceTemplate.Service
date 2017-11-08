using System.Threading.Tasks;

namespace MicroserviceTemplate.Service.Services.Interfaces
{
    public interface IHelloWorldService
    {
        Task<dynamic> HelloWorld();
        Task<dynamic> HelloWorldPost();
    }
}