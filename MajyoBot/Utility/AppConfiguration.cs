using System;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;

namespace MajyoBot.Utility
{
    public static class AppConfiguration
    {
        const string Production = "production";
        const string Development = "development";
        const string Test = "test";

        static string AppEnvironment
        {
            get
            {
                string env = Environment.GetEnvironmentVariable("MAJYOBOT_ENV");
                if (string.IsNullOrWhiteSpace(env))
                {
                    return Development;
                }

                return env;
            }
        }

        static AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            switch (AppEnvironment)
            {
                case Production:
                    configurationBuilder.AddIniFile("production.ini", optional: false, reloadOnChange: true);
                    break;
                case Test:
                    configurationBuilder.AddIniFile("testing.ini", optional: false, reloadOnChange: true);
                    break;
                case Development:
                default:
                    configurationBuilder.AddIniFile("development.ini", optional: false, reloadOnChange: true);
                    break;
            }

            configuration = configurationBuilder.Build();
        }

        static IConfiguration configuration;

        static IConfiguration Configuration
        {
            get
            {
                return configuration;
            }
        }

        static bool SafeParseBoolean(string val) 
        {
            bool.TryParse(val, out bool result);
            return result;
        }

        #region Configuration Sections

        public static class GoogleAuth 
        {
            static IConfigurationSection Section => Configuration.GetSection("GoogleAuth");
            public static string GoogleCredentialFile => Section["ApiCredentialFile"];
            public static GoogleCredential GoogleCredential => GoogleCredential.FromJson(GoogleCredentialFile);
        }

        public static class TelegramBot
        {
            static IConfigurationSection Section => Configuration.GetSection("TelegramBot");
            public static string ApiToken => Section["ApiToken"];
        }

        public static class Behavior 
        {
            static IConfigurationSection Section => Configuration.GetSection("Behavior");
            public static bool ShowFullError => SafeParseBoolean(Section["ShowFullError"]);
        }

        #endregion 

    }
}
