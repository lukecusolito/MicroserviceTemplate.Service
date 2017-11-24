using MicroserviceTemplate.Service.Logging;
using MicroserviceTemplate.Service.Utilities;
using MicroserviceTemplate.Service.Utilities.Configuration;
using MicroserviceTemplate.Service.Utilities.Pipelines;
using NLog;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceTemplate.Service.Test.Helpers
{
    public class TestSetup
    {
        public static PipelineHelper SetupPipelineHelper(Settings settingsMock = null, INLogger logger = null, IConfigurationManager configurationManager = null, ICorrelationId correlationId = null)
        {
            Nancy.Json.JsonSettings.RetainCasing = true;

            configurationManager = configurationManager != null ? configurationManager : Substitute.For<IConfigurationManager>();
            logger = logger != null ? logger : Substitute.For<INLogger>();
            correlationId = correlationId != null ? correlationId : Substitute.For<ICorrelationId>();

            settingsMock = settingsMock != null ? settingsMock : Entities.GetSettingsMockData();

            logger.When(x => x.Trace(Arg.Any<string>(), Arg.Any<Exception>(), Arg.Any<Guid>())).DoNotCallBase();
            logger.When(x => x.Error(Arg.Any<string>(), Arg.Any<Exception>(), Arg.Any<Guid>())).DoNotCallBase();

            configurationManager.Instance.Returns(settingsMock);

            return new PipelineHelper(configurationManager, logger, correlationId);
        }

        public static NLogger SetupNLogggerHelper(ILogger logger = null, IConfigurationManager configurationManager = null, Settings settingsMock = null)
        {
            configurationManager = configurationManager != null ? configurationManager : Substitute.For<IConfigurationManager>();
            logger = logger != null ? logger : Substitute.For<ILogger>();

            settingsMock = settingsMock != null ? settingsMock : Entities.GetSettingsMockData();

            logger.When(x => x.Log(Arg.Any<LogEventInfo>())).DoNotCallBase();

            configurationManager.Instance.Returns(settingsMock);

            return new NLogger(configurationManager, logger);
        }
    }
}
