using System;

namespace MicroserviceTemplate.Service.Utilities
{
    public interface ICorrelationId
    {
        Guid CurrentValue { get; set; }
    }
}
