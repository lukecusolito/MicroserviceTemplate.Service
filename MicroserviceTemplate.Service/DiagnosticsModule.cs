using MicroserviceTemplate.Service.Helpers;
using Nancy;
using System.Diagnostics;
using System.Reflection;

namespace MicroserviceTemplate.Service
{
    public class DiagnosticsModule : NancyModule
    {
        public DiagnosticsModule() : base("api/diagnostics")
        {
            Get["/isalive"] = x =>
            {
                string microserviceName = ConfigurationHelper.Instance.MicroserviceName;

                return new { MicroserviceName = microserviceName, IsAlive = true };
            };

            Get["/version"] = x =>
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

                string version = fvi.FileVersion;
                string microserviceName = ConfigurationHelper.Instance.MicroserviceName;

                // TODO: Add database connection status

                return new { MicroserviceName = ConfigurationHelper.Instance.MicroserviceName, Version = version };
            };
        }
    }
}