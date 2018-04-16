using MicroserviceTemplate.Service.Enumerations;
using MicroserviceTemplate.Service.Logging;
using MicroserviceTemplate.Service.Test.Helpers;
using MicroserviceTemplate.Service.Utilities;
using MicroserviceTemplate.Service.Utilities.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MicroserviceTemplate.Service.Test.UnitTests
{
    /// <summary>
    /// @Feature: Pipline 
    /// </summary>
    /// <remarks>
    /// As a developer 
    /// I want service request pipelines
    /// So that I can control the full request lifecycle
    /// </remarks>
    [TestClass]
    public class PipelineHelperTests
    {
        /// <summary>
        /// @Scenario: Get the correlation id when passed in the request
        /// </summary>
        /// <remarks>
        /// Given a request has been made to the service
        /// When the correlationId is supplied in the request
        /// Then the service should return extract the correlation id from the request
        /// </remarks>
        [TestMethod]
        public void GetTheCorrelationIdWhenPassedInTheRequest()
        {
            // Arrange
            var pipelineHelper = TestSetup.SetupPipelineHelper();
            var expected = Guid.NewGuid();

            var requestQuery = new JObject();
            requestQuery.Add("CorrelationId", expected.ToString());

            var nancyRequest = Entities.GenerateNancyRequest(query: requestQuery);

            // Act
            var actual = pipelineHelper.GetCorrelationId(nancyRequest);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// @Scenario: Get the correlation id when passed in the request body
        /// </summary>
        /// <remarks>
        /// Given a request has been made to the service
        /// When the correlationId is supplied in the request body
        /// Then the service should return extract the correlationId from the request body
        /// </remarks>
        [TestMethod]
        public void GetTheCorrelationIdWhenPassedInTheRequestBody()
        {
            // Arrange
            var pipelineHelper = TestSetup.SetupPipelineHelper();
            var expected = Guid.NewGuid();

            var requestBody = new JObject();
            requestBody.Add("CorrelationId", expected.ToString());

            var nancyRequest = Entities.GenerateNancyRequest(body: requestBody);

            // Act
            var actual = pipelineHelper.GetCorrelationId(nancyRequest);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// @Scenario: Get the correlation id from request query when also passed in the body
        /// </summary>
        /// <remarks>
        /// Given a request has been made to the service
        /// When the correlationId is supplied in the request query
        /// And the correlationId is supplied in the request body
        /// Then the service should return extract the correlationId from the request query
        /// </remarks>
        [TestMethod]
        public void GetTheCorrelationIdFromRequestQueryWhenAlsoPassedInTheBody()
        {
            // Arrange
            var pipelineHelper = TestSetup.SetupPipelineHelper();
            var expected = Guid.NewGuid();

            var requestQuery = new JObject();
            requestQuery.Add("CorrelationId", expected.ToString());

            var requestBody = new JObject();
            requestBody.Add("CorrelationId", Guid.NewGuid().ToString());

            var nancyRequest = Entities.GenerateNancyRequest(query: requestQuery, body: requestBody);

            // Act
            var actual = pipelineHelper.GetCorrelationId(nancyRequest);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// @Scenario: Set the correlation id when not passed in the request
        /// </summary>
        /// <remarks>
        /// Given a request has been made to the service
        /// When the correlationId has not been supplied to the service
        /// And a request correlation id is not required
        /// Then the service should create a correlation id
        /// </remarks>
        [TestMethod]
        public void SetTheCorrelationIdWhenNotPassedInTheRequest()
        {
            // Arrange
            var pipelineHelper = TestSetup.SetupPipelineHelper();

            var nancyRequest = Entities.GenerateNancyRequest();

            // Act
            var actual = pipelineHelper.GetCorrelationId(nancyRequest);

            // Assert
            Assert.IsTrue(actual != Guid.Empty);
        }

        /// <summary>
        /// @Scenario: Request fails when the correlation id is required and not passed through the request
        /// </summary>
        /// <remarks>
        /// Given a request has been made to the service
        /// When the correlationId has not been supplied to the service
        /// And a request correlation id is required
        /// Then the service should throw an exception
        /// </remarks>
        [TestMethod]
        public void RequestFailsWhenTheCorrelationIdIsRequiredAndNotPassedThroughTheRequest()
        {
            // Arrange
            var expected = ErrorCode.CORRELATIONID_REQUIRED.ToString();
            var configSettings = new Settings { RequestCorrelationIdIsRequired = true };
            var pipelineHelper = TestSetup.SetupPipelineHelper(configSettings);
            var nancyRequest = Entities.GenerateNancyRequest();

            try
            {
                // Act
                var actual = pipelineHelper.GetCorrelationId(nancyRequest);

                // Assert
                Assert.Fail("Exception was not thrown");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expected, ex.Message);
            }
        }

        /// <summary>
        /// @Scenario: Response message is logged for successful responses
        /// </summary>
        /// <remarks>
        /// Given a request has been made to the service
        /// When the service responds
        /// And no errors have been generated
        /// Then the service should log the response message
        /// And send http status code OK
        /// </remarks>
        [TestMethod]
        public void ResponseMessageIsLoggedForSuccessfulResponses()
        {
            // Arrange
            INLogger _logger = Substitute.For<INLogger>();
            var configSettings = new Settings { RequestCorrelationIdIsRequired = true };
            var pipelineHelper = TestSetup.SetupPipelineHelper(configSettings, logger: _logger);

            var responseBody = new JObject();
            responseBody.Add("Batman", "Begins");

            var response = new Nancy.Response()
            {
                StatusCode = Nancy.HttpStatusCode.OK,
                Contents = stream =>
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(responseBody);
                    }
                }
            };

            // Act
            pipelineHelper.LogAndFormatResponse(response);

            // Assert
            _logger.Received(1).Trace(Arg.Any<string>(), Arg.Any<Exception>(), Arg.Any<Guid>());
            Assert.AreEqual(Nancy.HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// @Scenario: Response message is logged for responses containing errors
        /// </summary>
        /// <remarks>
        /// Given a request has been made to the service
        /// When the service responds
        /// And errors are attached to the response
        /// Then the service should log the response message
        /// And send http status code Bad Request
        /// </remarks>
        [TestMethod]
        public void ResponseMessageIsLoggedForResponsesContainingErrors()
        {
            // Arrange
            INLogger _logger = Substitute.For<INLogger>();
            var configSettings = new Settings { RequestCorrelationIdIsRequired = true };
            var pipelineHelper = TestSetup.SetupPipelineHelper(configSettings, logger: _logger);

            var responseBody = JsonSerializer.ToJson(new
            {
                Batman = "Begins",
                Errors = new List<object> {
                    new {
                        ErrorCode = "ErrorCode",
                        ErrorMessage = "ErrorMessage"
                    }
                }
            });

            var response = new Nancy.Response()
            {
                StatusCode = Nancy.HttpStatusCode.OK,
                Contents = stream =>
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(responseBody);
                    }
                }
            };

            // Act
            pipelineHelper.LogAndFormatResponse(response);

            // Assert
            _logger.Received(1).Trace(Arg.Any<string>(), Arg.Any<Exception>(), Arg.Any<Guid>());
            Assert.AreEqual(Nancy.HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// @Scenario: Request message is logged
        /// </summary>
        /// <remarks>
        /// Given a request has been made to the service
        /// When the receives the call
        /// Then the service should log the request message
        /// </remarks>
        [TestMethod]
        public void RequestMessageIsLogged()
        {
            // Arrange
            INLogger _logger = Substitute.For<INLogger>();
            var pipelineHelper = TestSetup.SetupPipelineHelper(logger: _logger);

            var requestBody = new JObject();
            requestBody.Add("Batman", "Begins");

            var request = Entities.GenerateNancyRequest(body: requestBody);

            // Act
            pipelineHelper.LogRequest(request);

            // Assert
            _logger.Received(1).Trace(Arg.Any<string>(), Arg.Any<Exception>(), Arg.Any<Guid>());
        }

        /// <summary>
        /// @Scenario: Error response message is logged and error defaults when unexpected message supplied
        /// </summary>
        /// <remarks>
        /// Given a request fails
        /// When an error occurs
        /// And the error message is not one expected
        /// Then the service should set the error code and message to UNEXPECTED_ERROR
        /// And the service should log the response message
        /// </remarks>
        [TestMethod]
        public void ErrorResponseMessageIsLoggedAndErrorDefaultsWhenWnexpectedMessageSupplied()
        {
            // Arrange
            INLogger _logger = Substitute.For<INLogger>();
            var pipelineHelper = TestSetup.SetupPipelineHelper(logger: _logger);

            // Act
            var actual = pipelineHelper.BuildErrorResponse("Something went wrong");

            var actualPayload = Utils.ResponseToJObject(actual);

            // Assert
            _logger.Received(1).Trace(Arg.Any<string>(), Arg.Any<Exception>(), Arg.Any<Guid>());
            Assert.AreEqual(1, actualPayload["Errors"].ToList().Count);
            Assert.AreEqual(ErrorCode.UNEXPECTED_ERROR.ToString(), actualPayload["Errors"].ToList()[0]["ErrorCode"]);
            Assert.AreEqual("An unexpected error has occurred. Please try again later.", actualPayload["Errors"].ToList()[0]["ErrorMessage"]);
        }

        /// <summary>
        /// @Scenario: Error response message is logged and supplied error code returned to user
        /// </summary>
        /// <remarks>
        /// Given a request fails
        /// When an error occurs
        /// And the error message is not one expected
        /// Then the service should set the error code and message to UNEXPECTED_ERROR
        /// And the service should log the response message
        /// </remarks>
        [TestMethod]
        public void ErrorResponseMessageIsLoggedAndSuppliedErrorCodeReturnedToUser()
        {
            // Arrange
            INLogger _logger = Substitute.For<INLogger>();
            var pipelineHelper = TestSetup.SetupPipelineHelper(logger: _logger);

            // Act
            var actual = pipelineHelper.BuildErrorResponse(ErrorCode.CORRELATIONID_REQUIRED.ToString());

            var actualPayload = Utils.ResponseToJObject(actual);

            // Assert
            _logger.Received(1).Trace(Arg.Any<string>(), Arg.Any<Exception>(), Arg.Any<Guid>());
            Assert.AreEqual(1, actualPayload["Errors"].ToList().Count);
            Assert.AreEqual(ErrorCode.CORRELATIONID_REQUIRED.ToString(), actualPayload["Errors"].ToList()[0]["ErrorCode"]);
            Assert.AreEqual("CorrelationId must be passed.", actualPayload["Errors"].ToList()[0]["ErrorMessage"]);
        }

        /// <summary>
        /// @Scenario: Error response message is logged and supplied error code returned to user
        /// </summary>
        /// <remarks>
        /// Given a request fails
        /// When an error occurs
        /// And the error message is not one expected
        /// Then the service should set the error code and message to UNEXPECTED_ERROR
        /// And the service should log the response message
        /// </remarks>
        [TestMethod]
        public void ErrorMessageIsLogged()
        {
            // Arrange
            INLogger _logger = Substitute.For<INLogger>();
            var pipelineHelper = TestSetup.SetupPipelineHelper(logger: _logger);

            // Act
            pipelineHelper.LogError(new Exception());

            // Assert
            _logger.Received(1).Error(Arg.Any<string>(), Arg.Any<Exception>(), Arg.Any<Guid>());
        }

        /// <summary>
        /// @Scenario: Get the correlation id when passed in the request header
        /// </summary>
        /// <remarks>
        /// Given a request has been made to the service
        /// When the correlationId is supplied in the request header
        /// Then the service should return extract the correlation id from the request header
        /// </remarks>
        [TestMethod]
        public void GetTheCorrelationIdWhenPassedInTheRequestHeader()
        {
            // Arrange
            var pipelineHelper = TestSetup.SetupPipelineHelper();
            var expected = Guid.NewGuid();

            var requestHeader = new JObject();
            requestHeader.Add("CorrelationId", new JArray { expected.ToString() });

            var nancyRequest = Entities.GenerateNancyRequest(headers: requestHeader);

            // Act
            var actual = pipelineHelper.GetCorrelationId(nancyRequest);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
