using MicroserviceTemplate.Service.Logging;
using MicroserviceTemplate.Service.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceTemplate.Service.Test.UnitTests
{
    /// <summary>
    /// @Feature: Logging 
    /// </summary>
    /// <remarks>
    /// As a developer 
    /// I want service to implement logging
    /// So that I can track significant events
    /// </remarks>
    [TestClass]
    public class NLoggerTests
    {
        /// <summary>
        /// @Scenario: Log level trace reflects in the log message
        /// </summary>
        /// <remarks>
        /// Given the service has requested a message to be logged
        /// When the log level is trace
        /// Then the service logs should reflect that log level
        /// </remarks>
        [TestMethod]
        public void LogLevelTraceReflectsInTheLogMessage()
        {
            // Arrange
            ILogger _logger = Substitute.For<ILogger>();

            var nLogger = TestSetup.SetupNLogggerHelper(_logger);

            // Act
            nLogger.Trace("Log message");

            // Assert
            _logger.Received(1).Log(Arg.Is<LogEventInfo>(x => x.Level.Name == Level.Trace.ToString()));
        }

        /// <summary>
        /// @Scenario: Log level debug reflects in the log message
        /// </summary>
        /// <remarks>
        /// Given the service has requested a message to be logged
        /// When the log level is debug
        /// Then the service logs should reflect that log level
        /// </remarks>
        [TestMethod]
        public void LogLevelDebugReflectsInTheLogMessage()
        {
            // Arrange
            ILogger _logger = Substitute.For<ILogger>();

            var nLogger = TestSetup.SetupNLogggerHelper(_logger);

            // Act
            nLogger.Debug("Log message");

            // Assert
            _logger.Received(1).Log(Arg.Is<LogEventInfo>(x => x.Level.Name == Level.Debug.ToString()));
        }

        /// <summary>
        /// @Scenario: Log level info reflects in the log message
        /// </summary>
        /// <remarks>
        /// Given the service has requested a message to be logged
        /// When the log level is info
        /// Then the service logs should reflect that log level
        /// </remarks>
        [TestMethod]
        public void LogLevelInfoReflectsInTheLogMessage()
        {
            // Arrange
            ILogger _logger = Substitute.For<ILogger>();

            var nLogger = TestSetup.SetupNLogggerHelper(_logger);

            // Act
            nLogger.Info("Log message");

            // Assert
            _logger.Received(1).Log(Arg.Is<LogEventInfo>(x => x.Level.Name == Level.Info.ToString()));
        }

        /// <summary>
        /// @Scenario: Log level warn reflects in the log message
        /// </summary>
        /// <remarks>
        /// Given the service has requested a message to be logged
        /// When the log level is warn
        /// Then the service logs should reflect that log level
        /// </remarks>
        [TestMethod]
        public void LogLevelWarnReflectsInTheLogMessage()
        {
            // Arrange
            ILogger _logger = Substitute.For<ILogger>();

            var nLogger = TestSetup.SetupNLogggerHelper(_logger);

            // Act
            nLogger.Warn("Log message");

            // Assert
            _logger.Received(1).Log(Arg.Is<LogEventInfo>(x => x.Level.Name == Level.Warn.ToString()));
        }

        /// <summary>
        /// @Scenario: Log level error reflects in the log message
        /// </summary>
        /// <remarks>
        /// Given the service has requested a message to be logged
        /// When the log level is error
        /// Then the service logs should reflect that log level
        /// </remarks>
        [TestMethod]
        public void LogLevelErrorReflectsInTheLogMessage()
        {
            // Arrange
            ILogger _logger = Substitute.For<ILogger>();

            var nLogger = TestSetup.SetupNLogggerHelper(_logger);

            // Act
            nLogger.Error("Log message");

            // Assert
            _logger.Received(1).Log(Arg.Is<LogEventInfo>(x => x.Level.Name == Level.Error.ToString()));
        }

        /// <summary>
        /// @Scenario: Log level fatal reflects in the log message
        /// </summary>
        /// <remarks>
        /// Given the service has requested a message to be logged
        /// When the log level is fatal
        /// Then the service logs should reflect that log level
        /// </remarks>
        [TestMethod]
        public void LogLevelFatalReflectsInTheLogMessage()
        {
            // Arrange
            ILogger _logger = Substitute.For<ILogger>();

            var nLogger = TestSetup.SetupNLogggerHelper(_logger);

            // Act
            nLogger.Fatal("Log message");

            // Assert
            _logger.Received(1).Log(Arg.Is<LogEventInfo>(x => x.Level.Name == Level.Fatal.ToString()));
        }

        /// <summary>
        /// @Scenario: Message logged when no exception or correlationId passed
        /// </summary>
        /// <remarks>
        /// Given the service has requested a message to be logged
        /// When a message is supplied
        /// And there is no exception
        /// And there is no correlationId
        /// Then the logs should contain the message
        /// And a blank exception
        /// And a blank correlationId
        /// </remarks>
        [TestMethod]
        public void MessageLoggedWhenNoExceptionOrCorrelationIdPassed()
        {
            // Arrange
            ILogger _logger = Substitute.For<ILogger>();
            var expectedMessage = "Log message";

            var nLogger = TestSetup.SetupNLogggerHelper(_logger);

            // Act
            nLogger.Info(expectedMessage);

            // Assert
            _logger.Received(1).Log(Arg.Is<LogEventInfo>(x =>
                x.Message.Equals(expectedMessage) &&
                x.Properties["Exception"].Equals(string.Empty) &&
                x.Properties["CorrelationId"].Equals(Guid.Empty)));
        }

        /// <summary>
        /// @Scenario: Message and correlationId logged when no exception passed
        /// </summary>
        /// <remarks>
        /// Given the service has requested a message to be logged
        /// When a message is supplied with a correlationId
        /// And there is no exception
        /// Then the logs should contain the message
        /// And a blank exception
        /// And a correlationId
        /// </remarks>
        [TestMethod]
        public void MessageAndCorrelationIdLoggedWhenNoExceptionPassed()
        {
            // Arrange
            ILogger _logger = Substitute.For<ILogger>();
            var expectedMessage = "Log message";
            var expectedCorrelationId = Guid.NewGuid();

            var nLogger = TestSetup.SetupNLogggerHelper(_logger);

            // Act
            nLogger.Info(expectedMessage, correlationId: expectedCorrelationId);

            // Assert
            _logger.Received(1).Log(Arg.Is<LogEventInfo>(x =>
                x.Message.Equals(expectedMessage) &&
                x.Properties["Exception"].Equals(string.Empty) &&
                x.Properties["CorrelationId"].Equals(expectedCorrelationId)));
        }

        /// <summary>
        /// @Scenario: Message with correlationId and exception logged
        /// </summary>
        /// <remarks>
        /// Given the service has requested a message to be logged
        /// When a message is supplied with a correlationId and exception
        /// Then the logs should contain the message
        /// And a exception
        /// And a correlationId
        /// </remarks>
        [TestMethod]
        public void MessageWithCorrelationIdAndExceptionLogged()
        {
            // Arrange
            ILogger _logger = Substitute.For<ILogger>();
            var expectedMessage = "Log message";
            var expectedCorrelationId = Guid.NewGuid();

            var nLogger = TestSetup.SetupNLogggerHelper(_logger);

            // Act
            nLogger.Info(expectedMessage, new Exception("Something went wrong"), expectedCorrelationId);

            // Assert
            _logger.Received(1).Log(Arg.Is<LogEventInfo>(x =>
                x.Message.Equals(expectedMessage) &&
                (string)x.Properties["Exception"] != string.Empty &&
                x.Properties["CorrelationId"].Equals(expectedCorrelationId)));
        }

        /// <summary>
        /// @Scenario: Log event contains service specific properties
        /// </summary>
        /// <remarks>
        /// Given the service has requested a message to be logged
        /// When a message is supplied
        /// Then the logs should contain the ApplicationName
        /// And the CorrelationId
        /// And the MachineName
        /// And the MicroserviceName
        /// And the Exception
        /// </remarks>
        [TestMethod]
        public void LogEventContainsServiceSpecificProperties()
        {
            // Arrange
            ILogger _logger = Substitute.For<ILogger>();
            var expectedMessage = "Log message";
            var expectedCorrelationId = Guid.NewGuid();

            var nLogger = TestSetup.SetupNLogggerHelper(_logger);

            // Act
            nLogger.Info(expectedMessage, new Exception("Something went wrong"), expectedCorrelationId);

            // Assert
            _logger.Received(1).Log(Arg.Is<LogEventInfo>(x =>
                x.Properties.ContainsKey("ApplicationName") &&
                x.Properties.ContainsKey("CorrelationId") &&
                x.Properties.ContainsKey("MachineName") &&
                x.Properties.ContainsKey("MicroserviceName") &&
                x.Properties.ContainsKey("Exception")
                ));
        }
    }
}
