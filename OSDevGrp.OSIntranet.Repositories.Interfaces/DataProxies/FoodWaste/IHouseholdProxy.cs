using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to a household.
    /// </summary>
    public interface IHouseholdProxy : IHousehold, IMySqlDataProxy, IMySqlDataProxyCreator<IHouseholdProxy>
    {
    }
}
