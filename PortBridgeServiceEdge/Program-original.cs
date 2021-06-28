//This implementation is based on the appsettings.json file

//using AzureRelayPortBridge;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Microsoft.Azure.Devices.Client;
//using System.IO;
//using System.Threading.Tasks;
//using Newtonsoft.Json.Linq;
//using System.Text;
//using System;

//namespace PortBridgeServiceEdge
//{
//    class Program
//    {
//        private static ModuleClient s_moduleClient;
//        private static ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
//        private static ServiceCollection serviceCollection = new ServiceCollection();
//        private static ServiceProvider serviceProvider;

       
//        private static Task<MethodResponse> StartStopRelay(MethodRequest methodRequest, object userContext)
//        {
//            var data = methodRequest.DataAsJson;
//            JObject o = JObject.Parse(data);
//            // Check the payload
//            string command = o.GetValue("Command").ToString();
//            Console.WriteLine("received command {0}", command);
//            if (command == "start-relay")
//            {
//                Console.WriteLine("starting service");
//                serviceProvider.GetService<HybridConnectionServerHost>().Run().GetAwaiter().GetResult();
                
//                string result = $"{{\"result\":\"Executed direct method: {methodRequest.Name}\"}}";
//                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200));

//            }
//            else if (command == "stop-relay")
//            {
//                Console.WriteLine("stopping Service");
//                serviceProvider.GetService<HybridConnectionServerHost>().Stop().GetAwaiter().GetResult();

//                Console.WriteLine(serviceProvider.GetServices<HybridConnectionServerHost>().ToString());
//                string result = $"{{\"result\":\"Executed direct method: {methodRequest.Name}\"}}";
//                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200));
//            }
//            else
//            {
//                // Acknowlege the direct method call with a 400 error message
//                string result = "{\"result\":\"Invalid parameter\"}";
//                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 400));
//            }
//        }
//        static async Task Main(string[] args)
//        {
//            configurationBuilder
//                .SetBasePath(Directory.GetCurrentDirectory())
//                .AddJsonFile("appsettings.json", true)
//                .AddEnvironmentVariables()
//                .AddCommandLine(args);

//            var configuration = configurationBuilder.Build();

//            serviceCollection
//                .AddOptions()
//                .AddLogging(c => c.AddConsole())
//                .Configure<HybridConnectionServerOptions>(c => configuration.Bind("HybridConnectionServerHost", c))
//                .AddSingleton<HybridConnectionServerHost>();

//             serviceProvider = serviceCollection.BuildServiceProvider();
            
//            s_moduleClient = await ModuleClient.CreateFromEnvironmentAsync();
//            await s_moduleClient.SetMethodHandlerAsync("StartStopRelay", StartStopRelay, null);

//            while (true)
//                System.Threading.Thread.Sleep(1000);
//        }
//    }
//}
