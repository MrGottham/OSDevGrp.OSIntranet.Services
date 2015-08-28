using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Collection of food groups.
    /// </summary>
    public class FoodGroupCollection : Collection<IFoodGroup>, IFoodGroupCollection
    {
        #region Private variables

        private readonly IDataProvider _dataProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a collection of food groups.
        /// </summary>
        /// <param name="foodGroups">Food groups which should be in the collection.</param>
        /// <param name="dataProvider">Data provider who has provided the food groups.</param>
        public FoodGroupCollection(IEnumerable<IFoodGroup> foodGroups, IDataProvider dataProvider)
            : base(foodGroups.ToList())
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
        /// Gets the data provider who has provided the food groups.
        /// </summary>
        public virtual IDataProvider DataProvider
        {
            get { return _dataProvider; }
        }

        #endregion
    }
}
