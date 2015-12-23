using System;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.QueryHandlers.Core
{
    /// <summary>
    /// Functionality which handles a query for getting a specific static text.
    /// </summary>
    /// <typeparam name="TQuery">Type of the query for getting a specific static text.</typeparam>
    public abstract class StaticTextGetQueryHandlerBase<TQuery> : IQueryHandler<TQuery, StaticTextView> where TQuery : StaticTextGetQueryBase
    {
        #region Private variables

        private readonly ISystemDataRepository _systemDataRepository;
        private readonly IFoodWasteObjectMapper _foodWasteObjectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the functionality which handles a query for getting a specific static text.
        /// </summary>
        /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
        protected StaticTextGetQueryHandlerBase(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper)
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
        /// Gets the type for the specific static text to get.
        /// </summary>
        public abstract StaticTextType StaticTextType { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Functionality which handles a query for getting a specific static text.
        /// </summary>
        /// <param name="query">Query for getting the specific static text.</param>
        /// <returns>Static text.</returns>
        public virtual StaticTextView Query(TQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var translationInfo = _systemDataRepository.Get<ITranslationInfo>(query.TranslationInfoIdentifier);
            var staticText = _systemDataRepository.StaticTextGetByStaticTextType(StaticTextType);

            return _foodWasteObjectMapper.Map<IStaticText, StaticTextView>(staticText, translationInfo.CultureInfo);
        }

        #endregion
    }
}
