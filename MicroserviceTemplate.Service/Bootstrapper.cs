using MicroserviceTemplate.Service.Enumerations;
using MicroserviceTemplate.Service.Helpers;
using MicroserviceTemplate.Service.Logging;
using MicroserviceTemplate.Service.Resources;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.IO;
using Nancy.Responses;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.IO;

namespace MicroserviceTemplate.Service
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private NLogger Logger = new NLogger(typeof(Bootstrapper));
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            Nancy.Json.JsonSettings.RetainCasing = true;
        }
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            pipelines.BeforeRequest += (ctx) =>
            {
                if (ctx.Request.Path.StartsWith("/api"))
                {
                    CorrelationId.CurrentValue = GetCorrelationIdFromRequest(ctx.Request);
                    LogRequest(ctx.Request);
                    ctx.Request.Body.Position = 0;
                }
                return null;
            };

            pipelines.AfterRequest += (ctx) =>
            {
                if (ctx.Request.Path.StartsWith("/api") && ctx.Response.ContentType.Contains("json"))
                {
                    LogAndFormatResponse(ctx.Response);
                }
            };

            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                Logger.Error(ex.Message, ex, CorrelationId.CurrentValue);
                var serializer = new DefaultJsonSerializer();
                var error = BuildErrorResponse(ex.Message, serializer);
                error.StatusCode = HttpStatusCode.BadRequest;
                return error;
            });

            base.RequestStartup(container, pipelines, context);
        }

        #region Helper Methods
        private Guid GetCorrelationIdFromRequest(Request request)
        {
            if (request.Query["CorrelationId"] != null)
                return Guid.Parse(request.Query["CorrelationId"].ToString());

            return ReadCorrelationIdFromBody(request.Body);
        }

        private Guid ReadCorrelationIdFromBody(RequestStream body)
        {
            var result = Guid.NewGuid();
            var jsonObject = Utils.BodyToJObject(body);

            if (jsonObject != null && jsonObject.Property("CorrelationId") != null)
                return jsonObject.Property("CorrelationId").ToObject<Guid>();

            //throw new Exception("CORRELATIONID_REQUIRED");
            return result;
        }

        private void LogAndFormatResponse(Response response)
        {
            var jsonObject = Utils.ResponseToJObject(response);

            var responseToLog = new
            {
                Response = jsonObject
            };

            var responseString = JsonSerializer.ToJson(responseToLog);
            Logger.Trace(responseString, correlationId: CorrelationId.CurrentValue);

            if (jsonObject.Property("CorrelationId") == null)
            {
                response.Contents = stream =>
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        jsonObject.Add("CorrelationId", CorrelationId.CurrentValue);
                        writer.Write(jsonObject.ToString());
                    }
                };
            }

            var errorCount = jsonObject.Property("Errors")?.Value.ToObject<List<object>>().Count ?? 0;
            
                if (errorCount > 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
            }

        private void LogRequest(Request request)
        {
            var requestToLog = new
            {
                Method = request.Method,
                Url = request.Path,
                Query = request.Query,
                Body = Utils.BodyToJObject(request.Body)
            };

            var requestString = JsonSerializer.ToJson(requestToLog);
            Logger.Trace(requestString, correlationId: CorrelationId.CurrentValue);
        }

        private Response BuildErrorResponse(string message, DefaultJsonSerializer serializer)
        {
            var errorCode = ErrorCode.UNEXPECTED_ERROR;
            Enum.TryParse(message, out errorCode);

            var errorResult = new
            {
                CorrelationId = CorrelationId.CurrentValue,
                Errors = new List<object> {
                    new {
                        ErrorCode = errorCode.ToString(),
                        ErrorMessage = LocalisationMessage.GetErrorMsg(errorCode.ToString())
                    }
                }
            };

            return new JsonResponse(errorResult, serializer);
        }

        #endregion
    }
}