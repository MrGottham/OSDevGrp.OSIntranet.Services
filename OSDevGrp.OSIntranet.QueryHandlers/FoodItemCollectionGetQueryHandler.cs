using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Query handler which handles a query for getting the collection of food items for household usage.
    /// </summary>
    public class FoodItemCollectionGetQueryHandler : FoodItemCollectionGetQueryHandlerBase<FoodItemCollectionView>
    {
        #region Constructor

        /// <summary>
        /// Creates a query handler which handles a query for getting the collection of food items for household usage.
        /// </summary>
        /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
        public FoodItemCollectionGetQueryHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper) 
            : base(systemDataRepository, foodWasteObjectMapper)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether only active food items should be included.
        /// </summary>
        protected override bool OnlyActive
        {
            get { return true; }
        }

        #endregion
    }
}
