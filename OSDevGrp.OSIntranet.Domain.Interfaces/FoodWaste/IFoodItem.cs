using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a food item.
    /// </summary>
    public interface IFoodItem : ITranslatable, IForeignKeyable
    {
        /// <summary>
        /// Gets the primary food group for the food item.
        /// </summary>
        IFoodGroup PrimaryFoodGroup { get; }

        /// <summary>
        /// Gets or sets whether the food item is active.
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Gets the food groups which this food item belong to.
        /// </summary>
        IEnumerable<IFoodGroup> FoodGroups { get; }

        /// <summary>
        /// Adds a food group which this food item should belong to.
        /// </summary>
        /// <param name="foodGroup">Food group which this food item should belong to.</param>
        void FoodGroupAdd(IFoodGroup foodGroup);
    }
}
