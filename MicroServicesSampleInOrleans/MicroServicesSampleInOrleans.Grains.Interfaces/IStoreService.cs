using System.Threading.Tasks;

namespace MicroServicesSampleInOrleans.Store.Grains.Interfaces
{
    public interface IStoreService : Orleans.IGrainWithIntegerKey
    {
        Task<string> GetStoreItem(int id);
    }
}
