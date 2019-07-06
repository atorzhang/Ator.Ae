using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Common.Web.Config
{
    public class AppSettings
    {
        public static IConfiguration config = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();
        public static T GetAppSettings<T>(string key,string settingFilePath= "appsettings.json") where T : class, new()
        {
            var _config = config;
            if (settingFilePath != "appsettings.json")
            {
                _config = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = settingFilePath, ReloadOnChange = true })
                .Build();
            }
            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return appconfig;
        }

    }
}
