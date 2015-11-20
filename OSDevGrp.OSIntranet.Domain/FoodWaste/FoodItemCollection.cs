using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Collection of food items.
    /// </summary>
    public class FoodItemCollection : Collection<IFoodItem>, IFoodItemCollection
    {
        #region Private variables

        private readonly IDataProvider _dataProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a collection of food items.
        /// </summary>
        /// <param name="foodItems">Food items which should be in the collection.</param>
        /// <param name="dataProvider">Data provider who has provided the food items.</param>
        public FoodItemCollection(IEnumerable<IFoodItem> foodItems, IDataProvider dataProvider)
            : base(foodItems.ToList())
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            _dataProvider = dataProvider;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the data provider who has provided the food items.
        /// </summary>
        public virtual IDataProvider DataProvider
        {
            get { return _dataProvider; }
        }

        /// <summary>
        /// Removes inactive food items from the collection.
        /// </summary>
        public virtual void RemoveInactiveFoodItems()
        {
            Items.Where(foodItem => foodItem.IsActive == false).ToList().ForEach(foodItem => Remove(foodItem));
            foreach (var foodItem in Items)
            {
                foodItem.RemoveInactiveFoodGroups();
            }
        }

        #endregion
    }
}
