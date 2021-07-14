using System;
using System.Threading.Tasks;
using MicroServicesSampleInOrleans.Store.Grains;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace MicroServicesSampleInOrleans.Store
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            // https://www.microsoft.com/en-us/research/publication/orleans-distributed-virtual-actors-for-programmability-and-scalability/
            // https://dotnet.github.io/orleans/docs/tutorials_and_samples/tutorial_1.html
            try
            {
                Console.WriteLine($"Starting Store Silo (args: {string.Join(',', args)})");
                var host = await StartSilo();
                Console.WriteLine("\n\n Press Enter to terminate...\n\n");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "StoreCluster";
                    options.ServiceId = "StoreService";
                })
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(StoreService).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
