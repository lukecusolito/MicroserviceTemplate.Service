using System;

namespace MicroserviceTemplate.Service.Utilities
{
    public class CorrelationId : ICorrelationId
    {
        public Guid CurrentValue { get; set; }
    }
}