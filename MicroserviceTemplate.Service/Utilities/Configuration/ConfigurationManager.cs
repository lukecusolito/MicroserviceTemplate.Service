﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroserviceTemplate.Service.Utilities.Configuration
{
    public sealed class ConfigurationManager : IConfigurationManager
    {
        #region Variables

        private Settings _settings;

        #endregion

        #region Properties

        public Settings Instance
        {
            get
            {
                if (_settings == null)
                {
                    LoadFromWebConfig();
                }

                return _settings;
            }
        }

        #endregion

        #region Methods

        private void LoadFromWebConfig()
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;

            if (appSettings == null)
                throw new NullReferenceException("Unable to access appsettings");

            _settings = new Settings();
            _settings.MicroserviceName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            _settings.ApplicationName = System.Web.Hosting.HostingEnvironment.SiteName;
            _settings.RequestCorrelationIdIsRequired = bool.Parse(appSettings["RequestCorrelationIdIsRequired"]);
        }

        #endregion
    }
}