using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to a given storage.
    /// </summary>
    public interface IStorageProxy : IStorage, IMySqlDataProxy<IStorage>
    {
    }
}
