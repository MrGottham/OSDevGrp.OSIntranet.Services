using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to a food item.
    /// </summary>
    public interface IFoodItemProxy : IFoodItem, IMySqlDataProxy<IFoodItem>
    {
    }
}
