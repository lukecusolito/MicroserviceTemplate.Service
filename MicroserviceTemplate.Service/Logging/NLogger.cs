using Newtonsoft.Json.Linq;
using NLog;
using System;

namespace MicroserviceTemplate.Service.Logging
{
    public class NLogger
    {
        private const string MicroserviceName = "MicroserviceTemplate";
        private static readonly ILogger ClassLogger;
        private static readonly string MachineName;

        private readonly ILogger _logger;
        private readonly string _loggerTypeName;

        static NLogger()
        {
            ClassLogger = LogManager.GetLogger(typeof(NLogger).FullName);
            MachineName = Environment.MachineName;
        }
        public NLogger(Type callerType = null)
        {
            if (callerType == null)
            {
                _loggerTypeName = typeof(NLogger).FullName;
                _logger = ClassLogger;
            }
            else
            {
                _loggerTypeName = callerType.FullName;
                _logger = LogManager.GetLogger(_loggerTypeName);
            }
        }
        public void Info(string message, Exception ex = null, Guid correlationId = default(Guid))
        {
            Log(Level.Info, message, ex, correlationId);
        }
        public void Debug(string message, Exception ex = null, Guid correlationId = default(Guid))
        {
            Log(Level.Debug, message, ex, correlationId);
        }

        public void Warn(string message, Exception ex = null, Guid correlationId = default(Guid))
        {
            Log(Level.Warn, message, ex, correlationId);
        }

        public void Error(string message, Exception ex = null, Guid correlationId = default(Guid))
        {
            Log(Level.Error, message, ex, correlationId);
        }

        public void Fatal(string message, Exception ex = null, Guid correlationId = default(Guid))
        {
            Log(Level.Fatal, message, ex, correlationId);
        }

        public void Trace(string message, Exception ex = null, Guid correlationId = default(Guid))
        {
            Log(Level.Trace, message, ex, correlationId);
        }

        #region Helpers
        private void Log(Level level, string message, Exception ex = null, Guid correlationId = default(Guid))
        {
            var logEventModel = new LogEventModel
            {
                MachineName = MachineName,
                ApplicationName = MicroserviceName,
                CorrelationId = correlationId,
                Exception = ex == null ? string.Empty : ex.ToString(),
                Level = level,
                LoggerSource = _loggerTypeName,
                Message = message,
                MicroserviceName = MicroserviceName,
                TimeStamp = DateTime.Now
            };
            Log(logEventModel);
        }

        private void Log(LogEventModel logEventModel)
        {
            var logEvent = new LogEventInfo
            {
                Level = LogLevel.FromOrdinal((int)logEventModel.Level),
                LoggerName = logEventModel.LoggerSource,
                Message = logEventModel.Message,
                TimeStamp = logEventModel.TimeStamp
            };
            logEvent.Properties["ApplicationName"] = logEventModel.ApplicationName;
            logEvent.Properties["CorrelationId"] = logEventModel.CorrelationId;
            logEvent.Properties["MachineName"] = logEventModel.MachineName;
            logEvent.Properties["MicroserviceName"] = logEventModel.MicroserviceName;
            logEvent.Properties["Exception"] = logEventModel.Exception;

            var properties = logEventModel.CustomProperties as JObject;
            if (properties != null)
            {
                foreach (var property in properties.Properties())
                {
                    logEvent.Properties[property.Name] = property.Value;
                }
            }
            _logger.Log(logEvent);
        }
        #endregion
    }
}