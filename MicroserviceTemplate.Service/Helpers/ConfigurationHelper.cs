using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroserviceTemplate.Service.Helpers
{
    public sealed class ConfigurationHelper
    {
        #region Variables

        private static readonly ConfigurationHelper instance = new ConfigurationHelper();

        #endregion

        #region Constructor

        private ConfigurationHelper()
        {
            LoadFromWebConfig();
        }
        static ConfigurationHelper() { }

        #endregion

        #region Properties

        public string MicroserviceName { get; private set; }

        public static ConfigurationHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region Methods

        private void LoadFromWebConfig()
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;

            if (appSettings == null)
                throw new NullReferenceException("Unable to access appsettings");

            MicroserviceName = appSettings["MicroserviceName"];
        }

        #endregion
    }
}