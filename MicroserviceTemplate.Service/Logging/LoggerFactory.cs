using MicroserviceTemplate.Service.Utilities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroserviceTemplate.Service.Logging
{
    public class NLoggerFactory : INLoggerFactory
    {
        private readonly IConfigurationManager _configurationManager;

        public NLoggerFactory(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public NLogger CreateLogger<T>() where T : class
        {
            return new NLogger(_configurationManager, typeof(T));
        }
    }
}