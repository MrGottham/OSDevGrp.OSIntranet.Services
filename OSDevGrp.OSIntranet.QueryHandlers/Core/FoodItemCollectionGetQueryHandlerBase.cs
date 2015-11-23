using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
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
        /// Gets whether only active food items should be included.
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

            var translationInfo = _systemDataRepository.Get<ITranslationInfo>(query.TranslationInfoIdentifier);
            var dataProvider = _systemDataRepository.DataProviderForFoodItemsGet();

            IFoodGroup foodGroup = null;
            if (query.FoodGroupIdentifier.HasValue)
            {
                foodGroup = _systemDataRepository.Get<IFoodGroup>(query.FoodGroupIdentifier.Value);
            }

            dataProvider.Translate(translationInfo.CultureInfo);

            var foodItemCollection = foodGroup == null
                ? GetFoodItemsForDataProvider(_systemDataRepository.FoodItemGetAll(), dataProvider)
                : GetFoodItemsForDataProvider(_systemDataRepository.FoodItemGetAllForFoodGroup(foodGroup), dataProvider);
            if (OnlyActive)
            {
                foodItemCollection.RemoveInactiveFoodItems();
            }

            return _foodWasteObjectMapper.Map<IFoodItemCollection, TFoodItemCollectionView>(foodItemCollection, translationInfo.CultureInfo);
        }

        /// <summary>
        /// Gets a collection of food items where a given data provider has a foreign key.
        /// </summary>
        /// <param name="foodItems">Collection of food items which should be filtered.</param>
        /// <param name="dataProvider">Data provider who should have a foreign key.</param>
        /// <returns>Collection of food items where the given data provider has a foreign key.</returns>
        private static IFoodItemCollection GetFoodItemsForDataProvider(IEnumerable<IFoodItem> foodItems, IDataProvider dataProvider)
        {
            if (foodItems == null)
            {
                throw new ArgumentNullException("foodItems");
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            return new FoodItemCollection(foodItems.Where(foodItem => foodItem.ForeignKeys != null && foodItem.ForeignKeys.Any(foreignKey => foreignKey.DataProvider != null && foreignKey.DataProvider.Identifier == dataProvider.Identifier)).ToList(), dataProvider);
        }

        #endregion
    }
}
