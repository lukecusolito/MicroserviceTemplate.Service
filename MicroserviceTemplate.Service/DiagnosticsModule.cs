using MicroserviceTemplate.Service.Utilities.Configuration;
using Nancy;
using System.Diagnostics;
using System.Reflection;

namespace MicroserviceTemplate.Service
{
    public class DiagnosticsModule : NancyModule
    {
        private readonly IConfigurationManager _configuration;
        public DiagnosticsModule(IConfigurationManager configuration) : base("api/diagnostics")
        {
            _configuration = configuration;

            Get["/isalive"] = parameters => new { MicroserviceName = _configuration.Instance.MicroserviceName, IsAlive = true };

            Get["/version"] = parameters =>
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string version = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;

                return new
                {
                    MicroserviceName = _configuration.Instance.MicroserviceName,
                    Version = version
                };
            };
        }
    }
}