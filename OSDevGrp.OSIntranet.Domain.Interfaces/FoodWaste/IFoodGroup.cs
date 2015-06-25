using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a food group.
    /// </summary>
    public interface IFoodGroup : ITranslatable, IForeignKeyable
    {
        /// <summary>
        /// Food group which has this food group as a child.
        /// </summary>
        IFoodGroup Parent { get; set; }

        /// <summary>
        /// Indicates whether the food group is active.
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Foods groups which has this food group as a parent. 
        /// </summary>
        IEnumerable<IFoodGroup> Children { get; } 
    }
}
