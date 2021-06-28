using AzureRelayPortBridge;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Devices.Client;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text;
using System;
using System.Linq;

namespace PortBridgeServiceEdge
{
    class Program
    {
        private static ModuleClient s_moduleClient;
        private static readonly ServiceCollection serviceCollection = new ServiceCollection();
        private static ServiceProvider serviceProvider;
      
        private static Task<MethodResponse> StopRelay(MethodRequest methodRequest, object userContext)
        {
            Console.WriteLine("stopping Service");
            serviceProvider.GetService<HybridConnectionServerHost>().Stop().GetAwaiter().GetResult();
            serviceCollection.Clear();
            Console.WriteLine(serviceProvider.GetServices<HybridConnectionServerHost>().ToString());
            string result = $"{{\"result\":\"Executed direct method: {methodRequest.Name}\"}}";
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200));
        }

        private static Task<MethodResponse> StartRelay(MethodRequest methodRequest, object userContext)
        {
            string data = methodRequest.DataAsJson;
            var configuration = new ConfigurationBuilder().AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(data))).Build();
            serviceCollection
                .AddOptions()
                .AddLogging(c => c.AddConsole())
                .Configure<HybridConnectionServerOptions>(c => configuration.Bind("HybridConnectionServerHost", c))
                .AddSingleton<HybridConnectionServerHost>();
             serviceProvider = serviceCollection.BuildServiceProvider( );
            Console.WriteLine("starting service");
            serviceProvider.GetService<HybridConnectionServerHost>().Run().GetAwaiter().GetResult();
            string result = $"{{\"result\":\"Executed direct method: {methodRequest.Name}\"}}";
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200));
        }

        static async Task Main(string[] args)
        {
            s_moduleClient = await ModuleClient.CreateFromEnvironmentAsync();
            await s_moduleClient.SetMethodHandlerAsync("StopRelay", StopRelay, null);
            await s_moduleClient.SetMethodHandlerAsync("StartRelay", StartRelay, null);

            while (true)
                System.Threading.Thread.Sleep(1000);
        }

       
    }
}
