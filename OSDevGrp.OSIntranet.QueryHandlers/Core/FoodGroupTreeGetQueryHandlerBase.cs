using System;
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
    /// Functionality which handles a query for getting the tree of food groups.
    /// </summary>
    /// <typeparam name="TFoodGroupTreeView">Type of the view to return.</typeparam>
    public abstract class FoodGroupTreeGetQueryHandlerBase<TFoodGroupTreeView> : IQueryHandler<FoodGroupTreeGetQuery, TFoodGroupTreeView> where TFoodGroupTreeView : IView
    {
        #region Private variables

        private readonly ISystemDataRepository _systemDataRepository;
        private readonly IFoodWasteObjectMapper _foodWasteObjectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates functionality which handles a query for getting the tree of food groups.
        /// </summary>
        /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
        protected FoodGroupTreeGetQueryHandlerBase(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper)
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
        /// Functionality which handles a query for getting the tree of food groups.
        /// </summary>
        /// <param name="query">Query for getting the tree of food groups.</param>
        /// <returns>Tree of food groups.</returns>
        public virtual TFoodGroupTreeView Query(FoodGroupTreeGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var translationInfo = _systemDataRepository.Get<ITranslationInfo>(query.TranslationInfoIdentifier);
            var dataProvider = _systemDataRepository.DataProviderForFoodGroupsGet();
            var foodGroups = _systemDataRepository.FoodGroupGetAllOnRoot()
                .Where(foodGroup => foodGroup.ForeignKeys != null && foodGroup.ForeignKeys.Any(foreignKey => foreignKey.DataProvider != null && foreignKey.DataProvider.Identifier == dataProvider.Identifier))
                .ToList();

            dataProvider.Translate(translationInfo.CultureInfo);

            var foodGroupCollection = new FoodGroupCollection(foodGroups, dataProvider);
            if (OnlyActive)
            {
                foodGroupCollection.RemoveInactiveFoodGroups();
            }

            return _foodWasteObjectMapper.Map<IFoodGroupCollection, TFoodGroupTreeView>(foodGroupCollection, translationInfo.CultureInfo);
        }

        #endregion
    }
}
