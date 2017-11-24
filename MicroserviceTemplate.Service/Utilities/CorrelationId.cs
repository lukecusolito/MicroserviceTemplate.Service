using MicroserviceTemplate.Service.Utilities;
using System;

namespace MicroserviceTemplate.Service.Helpers
{
    public class CorrelationId : ICorrelationId
    {
        public Guid CurrentValue { get; set; }
    }
}