using System;
using System.Threading.Tasks;
using MicroServicesSampleInOrleans.Store.Grains.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;

namespace MicroServicesSampleInOrleans.Client
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            try
            {
                Console.WriteLine($"Starting Client (args: {string.Join(',', args)})");
                await using var storeClient = await ConnectStoreClient();
                do
                {
                    await DoClientWork(storeClient);
                } while (Console.ReadKey().Key != ConsoleKey.Q);

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectStoreClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "StoreCluster";
                    options.ServiceId = "StoreService";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }

        private static async Task DoClientWork(IClusterClient storeClient)
        {
            for (int excCircuitBreaker = 0; excCircuitBreaker < 3; excCircuitBreaker++)
            {
                try
                {
                    // example of calling grains from the initialized client
                    var friend = storeClient.GetGrain<IStoreService>(0);

                    await CallGetStoreItemAndPrintResult(friend, 0);
                    await CallGetStoreItemAndPrintResult(friend, 1);
                    await CallGetStoreItemAndPrintResult(friend, 2);
                    await CallGetStoreItemAndPrintResult(friend, 3);
                    await CallGetStoreItemAndPrintResult(friend, 4);


                    break;
                }
                catch (Exception exc)
                {
                    await Console.Error.WriteLineAsync("EXCEPTION occurred: " + exc.Message);
                }
            }
        }

        private static async Task CallGetStoreItemAndPrintResult(IStoreService store, int id)
        {
            var storeResponse = await store.GetStoreItem(id) ?? "<null>";
            Console.WriteLine($"Calling GetStoreItem with id: {id}: result = {storeResponse}");
        }
    }
}
