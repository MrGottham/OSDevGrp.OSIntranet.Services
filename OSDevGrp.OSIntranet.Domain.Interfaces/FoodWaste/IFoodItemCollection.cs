using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a collection of food items.
    /// </summary>
    public interface IFoodItemCollection : ICollection<IFoodItem>
    {
        /// <summary>
        /// Gets the data provider who has provided the food items.
        /// </summary>
        IDataProvider DataProvider { get; }

        /// <summary>
        /// Removes inactive food items from the collection.
        /// </summary>
        void RemoveInactiveFoodItems();
    }
}
