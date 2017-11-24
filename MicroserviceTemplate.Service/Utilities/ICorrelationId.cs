using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceTemplate.Service.Utilities
{
    public interface ICorrelationId
    {
        Guid CurrentValue { get; set; }
    }
}
