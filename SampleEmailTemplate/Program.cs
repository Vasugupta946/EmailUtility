using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace EmailUtility
{
    class Program
    {
        private static readonly IConfigurationRoot config;
        static Program()
        {
            config = new ConfigurationBuilder()
                         .AddJsonFile("appsettings.json")
                         .Build();
        }
        static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<MailLogic>();
                })
                .ConfigureLogging(logBuilder =>
                {
                    logBuilder.SetMinimumLevel(LogLevel.Trace);
                    logBuilder.AddLog4Net("log4net.config");
                }).UseConsoleLifetime();

            var host = builder.Build();
            string filePath = config["AppSettings:Path"];

            using var serviceScope = host.Services.CreateScope();
            var service = serviceScope.ServiceProvider;
            var myService = service.GetRequiredService<MailLogic>();
            myService.GetFiles(filePath);
        }
    } 
}

