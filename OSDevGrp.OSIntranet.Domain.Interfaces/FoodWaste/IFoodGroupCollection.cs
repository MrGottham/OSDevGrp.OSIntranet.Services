using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a collection of food groups.
    /// </summary>
    public interface IFoodGroupCollection : ICollection<IFoodGroup>
    {
        /// <summary>
        /// Gets the data provider who has provided the food groups.
        /// </summary>
        IDataProvider DataProvider { get; }
    }
}
