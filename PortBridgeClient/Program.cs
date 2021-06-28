using AzureRelayPortBridge;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

namespace PortBridgeClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var serviceCollection = new ServiceCollection();

            configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
              
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables();

            var configuration = configurationBuilder.Build();

            serviceCollection
                .AddOptions()
                .AddLogging(c => c.AddConsole())
                //HybridConnectionClientHost is equivalent to Service Proxy
                //HybridConnectionServerHost  is equivalent to Device Proxy
                .Configure<HybridConnectionClientOptions>(c => configuration.Bind("HybridConnectionClientHost", c))
                .AddSingleton<HybridConnectionClientHost>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<HybridConnectionClientHost>().Run().GetAwaiter().GetResult();

            while (true)

                System.Threading.Thread.Sleep(1000);
        }
    }
}
