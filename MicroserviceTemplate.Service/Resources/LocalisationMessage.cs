using System.Reflection;
using System.Resources;

namespace MicroserviceTemplate.Service.Resources
{
    public static class LocalisationMessage
    {
        /// <summary>
        /// Get Error message value from Resource File non-prperty specific.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static string GetErrorMsg(string errorCode)
        {
            ResourceManager rm = new ResourceManager("MicroserviceTemplate.Service.Resources.Resource", Assembly.GetExecutingAssembly());
            return rm.GetString(errorCode);
        }
    }
}