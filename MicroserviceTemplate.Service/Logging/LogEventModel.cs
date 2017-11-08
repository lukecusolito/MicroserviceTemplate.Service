using System;

namespace MicroserviceTemplate.Service.Logging
{
    public class LogEventModel
    {
        /// <summary>
        ///     The entry point correlation unique identifier assigned to the request
        /// </summary>
        public Guid CorrelationId { get; set; }

        /// <summary>
        ///     The timestamp of the logging event. i.e. 2015-10-09T02:44:42.266Z
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        ///     The level of the logging event.
        /// </summary>
        public Level Level { get; set; }

        /// <summary>
        ///     The exception information. Usually the .ToString() of exception object.
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        ///     The name of the application where the log event occurred. e.g. IIS application name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        ///     The name of the microservice where the log event occurred.
        /// </summary>
        public string MicroserviceName { get; set; }

        /// <summary>
        ///     The name of the machine where the log event occurred.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        ///     The log message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     The name of the component/class that triggered the log event.
        /// </summary>
        public string LoggerSource { get; set; }


        /// <summary>
        ///     The object that holds custom properties related to the event.
        /// </summary>
        public object CustomProperties { get; set; }
    }
}