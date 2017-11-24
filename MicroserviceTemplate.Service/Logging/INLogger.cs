using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceTemplate.Service.Logging
{
    public interface INLogger
    {
        void Info(string message, Exception ex = null, Guid correlationId = default(Guid));
        void Debug(string message, Exception ex = null, Guid correlationId = default(Guid));
        void Warn(string message, Exception ex = null, Guid correlationId = default(Guid));
        void Error(string message, Exception ex = null, Guid correlationId = default(Guid));
        void Fatal(string message, Exception ex = null, Guid correlationId = default(Guid));
        void Trace(string message, Exception ex = null, Guid correlationId = default(Guid));
    }
}
