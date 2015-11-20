using System;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.QueryHandlers.Core
{
    /// <summary>
    /// Functionality which handles a query for getting the collection of food items.
    /// </summary>
    public abstract class FoodItemCollectionGetQueryHandlerBase<TFoodItemCollectionView> : IQueryHandler<FoodItemCollectionGetQuery, TFoodItemCollectionView> where TFoodItemCollectionView : IView
    {
        #region Private variables

        private readonly ISystemDataRepository _systemDataRepository;
        private readonly IFoodWasteObjectMapper _foodWasteObjectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates functionality which handles a query for getting the collection of food items.
        /// </summary>
        /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
        protected FoodItemCollectionGetQueryHandlerBase(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper)
        {
            if (systemDataRepository == null)
            {
                throw new ArgumentNullException("systemDataRepository");
            }
            if (foodWasteObjectMapper == null)
            {
                throw new ArgumentNullException("foodWasteObjectMapper");
            }
            _systemDataRepository = systemDataRepository;
            _foodWasteObjectMapper = foodWasteObjectMapper;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether only active food groups should be included.
        /// </summary>
        protected abstract bool OnlyActive { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Functionality which handles a query for getting the collection of food items.
        /// </summary>
        /// <param name="query">Query for getting the collection of food items.</param>
        /// <returns>Collection of food items.</returns>
        public virtual TFoodItemCollectionView Query(FoodItemCollectionGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}
