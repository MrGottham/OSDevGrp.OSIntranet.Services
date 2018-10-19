using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to a given food group.
    /// </summary>
    public interface IFoodGroupProxy : IFoodGroup, IMySqlDataProxy, IMySqlDataProxyCreator<IFoodGroupProxy>
    {
    }
}