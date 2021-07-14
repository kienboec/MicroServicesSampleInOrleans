using System;
using System.Threading.Tasks;
using MicroServicesSampleInOrleans.Store.Grains.Interfaces;
using Microsoft.Extensions.Logging;

namespace MicroServicesSampleInOrleans.Store.Grains
{
    public class StoreService : Orleans.Grain, IStoreService
    {
        private readonly ILogger<StoreService> logger;

        public StoreService(ILogger<StoreService> logger)
        {
            this.logger = logger;
        }

        public async Task<string> GetStoreItem(int id)
        {
            this.logger.LogInformation($"{nameof(GetStoreItem)} called with id:{id} ({DateTime.Now:O})");
            return await Task.FromResult(id switch
            {
                0 => "First Item",
                1 => "Second Item",
                2 => "Third Item",
                3 => "Fourth Item",
                _ => null
            });
        }
    }
}
