using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceTemplate.Service.Utilities.Configuration
{
    public interface IConfigurationManager
    {
        Settings Instance { get; }
    }
}
