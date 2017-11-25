using MicroserviceTemplate.Service.Enumerations;
using MicroserviceTemplate.Service.Logging;
using MicroserviceTemplate.Service.Models.Request;
using MicroserviceTemplate.Service.Resources;
using MicroserviceTemplate.Service.Utilities.Configuration;
using Nancy;
using Nancy.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MicroserviceTemplate.Service.Utilities.Pipelines
{
    public class PipelineHelper : IPipelineHelper
    {
        #region Variables

        private readonly IConfigurationManager _configuration;
        private readonly INLogger _logger;
        private readonly ICorrelationId _correlationId;

        #endregion

        #region Constructor(s)

        public PipelineHelper(IConfigurationManager configuration, INLoggerFactory loggerFactory, ICorrelationId correlationId)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<Bootstrapper>();
            _correlationId = correlationId;
        }

        public PipelineHelper(IConfigurationManager configuration, INLogger logger, ICorrelationId correlationId)
        {
            _configuration = configuration;
            _logger = logger;
            _correlationId = correlationId;
        }

        #endregion

        #region Public Methods

        public virtual Guid GetCorrelationId(NancyRequest request)
        {
            Guid correlationId = GetCorrelationIdFromRequest(request);

            if (correlationId == Guid.Empty)
            {
                if (_configuration.Instance.RequestCorrelationIdIsRequired)
                    throw new Exception(ErrorCode.CORRELATIONID_REQUIRED.ToString());
                else
                    correlationId = Guid.NewGuid();
            }

            return correlationId;
        }

        public virtual void LogAndFormatResponse(Response response)
        {
            var jsonObject = Utils.ResponseToJObject(response);

            var responseString = JsonSerializer.ToJson(jsonObject);
            _logger.Trace(responseString, correlationId: _correlationId.CurrentValue);

            var errorCount = jsonObject.Property("Errors")?.Value.ToObject<List<object>>().Count ?? 0;

            if (errorCount > 0)
                response.StatusCode = HttpStatusCode.BadRequest;
        }

        public virtual void LogRequest(NancyRequest request)
        {
            var requestString = JsonSerializer.ToJson(request);
            _logger.Trace(requestString, correlationId: _correlationId.CurrentValue);
        }

        public Response BuildErrorResponse(string message)
        {
            var errorCode = ErrorCode.UNEXPECTED_ERROR;
            Enum.TryParse(message, out errorCode);

            var errorResult = new
            {
                Errors = new List<object> {
                    new {
                        ErrorCode = errorCode.ToString(),
                        ErrorMessage = LocalisationMessage.GetErrorMsg(errorCode.ToString())
                    }
                }
            };

            var responseString = JsonSerializer.ToJson(errorResult);
            _logger.Trace(responseString, correlationId: _correlationId.CurrentValue);

            return new JsonResponse(errorResult, new DefaultJsonSerializer());
        }

        public void LogError(Exception ex)
        {
            _logger.Error(ex.Message, ex, _correlationId.CurrentValue);
        }

        #endregion

        #region Private Methods

        private Guid GetCorrelationIdFromRequest(NancyRequest request)
        {
            if (request.Query["CorrelationId"] != null)
                return Guid.Parse(request.Query["CorrelationId"].ToString());

            return ReadCorrelationIdFromBody(request.Body);
        }

        private Guid ReadCorrelationIdFromBody(JObject jsonObject)
        {
            if (jsonObject != null && jsonObject.Property("CorrelationId") != null)
                return jsonObject.Property("CorrelationId").ToObject<Guid>();

            return Guid.Empty;
        }

        #endregion
    }
}