using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceTemplate.Service.Logging
{
    public interface INLoggerFactory
    {
        NLogger CreateLogger<T>() where T : class;
    }
}
