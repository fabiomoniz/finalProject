using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Historian_Frame
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            //var config =  new ConfigurationBuilder().AddJsonFile("config.json").Build();
            var config = Configuration.ReadFrom("config.json");
            
            MasterController2 ctrl = new MasterController2();
            ctrl.cfg = (Configuration)config;

            ctrl.Run();

            //var host = new HostBuilder()
            //    .ConfigureAppConfiguration(configurationBuilder => { configurationBuilder.AddJsonFile("config.json"); })
            //    .ConfigureServices((hostContext, serviceCollection) =>
            //    {
            //        serviceCollection.AddSingleton(provider =>
            //        {
            //            var configuration = new Configuration();
            //            hostContext.Configuration.Bind(configuration);

            //            return configuration;
            //        });
            //        serviceCollection.AddSingleton<InfluxDbDataTransfere>();
            //        serviceCollection.AddHostedService<MasterController2>();
            //    })
            //    .ConfigureLogging(((hostContext, loggingBuilder) =>
            //    {
            //        loggingBuilder.AddConsole();
            //        loggingBuilder.AddDebug();
            //        loggingBuilder.AddLog4Net();
            //    }))
            //    .UseConsoleLifetime()
            //    .Build();

            //await host.RunAsync();
        }

    }
}
