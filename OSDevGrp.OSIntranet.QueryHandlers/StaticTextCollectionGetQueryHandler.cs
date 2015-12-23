using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Functionality which handles the query for getting a collection of static texts.
    /// </summary>
    public class StaticTextCollectionGetQueryHandler : IQueryHandler<StaticTextCollectionGetQuery, IEnumerable<StaticTextSystemView>>
    {
        #region Private variables

        private readonly ISystemDataRepository _systemDataRepository;
        private readonly IFoodWasteObjectMapper _foodWasteObjectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the functionality which handles the query for getting a collection of static texts.
        /// </summary>
        /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
        public StaticTextCollectionGetQueryHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper)
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

        #region Methods

        /// <summary>
        /// Functionality which handles the query for getting a collection of static texts.
        /// </summary>
        /// <param name="query">Query for getting a collection of static texts.</param>
        /// <returns>Collection of static texts.</returns>
        public virtual IEnumerable<StaticTextSystemView> Query(StaticTextCollectionGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var translationInfo = _systemDataRepository.Get<ITranslationInfo>(query.TranslationInfoIdentifier);
            var staticTexts = _systemDataRepository.StaticTextGetAll();

            return _foodWasteObjectMapper.Map<IEnumerable<IStaticText>, IEnumerable<StaticTextSystemView>>(staticTexts, translationInfo.CultureInfo);
        }

        #endregion
    }
}
