using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to a given relation between a food item and a food group in the food waste domain.
    /// </summary>
    public interface IFoodItemGroupProxy : IIdentifiable, IMySqlDataProxy<IFoodItemGroupProxy>
    {
        /// <summary>
        /// Gets the food item.
        /// </summary>
        IFoodItem FoodItem { get; }

        /// <summary>
        /// Gets or sets the identifier for the food item.
        /// </summary>
        Guid? FoodItemIdentifier { get; set; }

        /// <summary>
        /// Gets the food group.
        /// </summary>
        IFoodGroup FoodGroup { get; }

        /// <summary>
        /// Gets or sets the identifier for the food group.
        /// </summary>
        Guid? FoodGroupIdentifier { get; set; }

        /// <summary>
        /// Gets or sets whether this will be the primary food group for the food item.
        /// </summary>
        bool IsPrimary { get; set; }
    }
}
