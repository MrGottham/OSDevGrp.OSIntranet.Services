using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to a given storage type.
    /// </summary>
    public interface IStorageTypeProxy : IStorageType, IMySqlDataProxy, IMySqlDataProxyCreator<IStorageTypeProxy>
    {
    }
}
