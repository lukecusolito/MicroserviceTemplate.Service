using System;

namespace MicroserviceTemplate.Service.Helpers
{
    public static class CorrelationId
    {
        public static Guid CurrentValue { get; set; }
    }
}