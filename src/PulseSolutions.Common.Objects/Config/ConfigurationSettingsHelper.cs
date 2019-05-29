
using System.Configuration;

namespace PulseSolutions.Common.Objects.Config
{
    internal static class ConfigurationSettingsHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static string SaveLogEndpoint
        {
            get
            {
                return GetValue(nameof(SaveLogEndpoint));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string LoggerApiBaseAddress
        {
            get
            {
                return GetValue("LogManagerBaseAddress");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string InstanceName
        {
            get
            {
                return GetValue(nameof(InstanceName));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        private static string GetValue(string settingName)
        {
            string str = string.Empty;
            if (ConfigurationManager.AppSettings[settingName] != null)
                str = ConfigurationManager.AppSettings[settingName];
            return str;
        }
    }
}
