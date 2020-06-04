using System.IO;
using Microsoft.Extensions.Configuration;
using System;

namespace MedCore_DataAccess.Util
{
    public static class ConfigurationProvider
    {
        private static readonly IConfigurationRoot Configuration;

        static ConfigurationProvider()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public static string Get(string name)
        {
            return Configuration[name];
        }
    }
}