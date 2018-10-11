using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to a given data provider.
    /// </summary>
    public interface IDataProviderProxy : IDataProvider, IMySqlDataProxy, IMySqlDataProxyCreator<IDataProviderProxy>
    {
    }
}
