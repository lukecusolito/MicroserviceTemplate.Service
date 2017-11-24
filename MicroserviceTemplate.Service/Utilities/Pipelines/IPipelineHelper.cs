using MicroserviceTemplate.Service.Models.Request;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroserviceTemplate.Service.Utilities.Pipelines
{
    public interface IPipelineHelper
    {
        Guid GetCorrelationId(NancyRequest request);
        void LogRequest(NancyRequest request);
        void LogAndFormatResponse(Response response);
        Response BuildErrorResponse(string message);
        void LogError(Exception ex);

    }
}