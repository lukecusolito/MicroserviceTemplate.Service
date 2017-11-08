using Nancy;
using System.Diagnostics;
using System.Reflection;

namespace MicroserviceTemplate.Service
{
    public class DiagnosticsModule : NancyModule
    {
        public DiagnosticsModule() : base("api/diagnostics")
        {
            Get["/isalive"] = x => new { isAlive = true };

            Get["/version"] = x =>
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;
                return new { version };
            };
        }
    }
}