using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroserviceTemplate.Service.Utilities.Configuration
{
    public class Settings
    {
        public string MicroserviceName { get; set; }
        public bool RequestCorrelationIdIsRequired { get; set; }
        public string ApiPrefix { get; set; }
    }
}