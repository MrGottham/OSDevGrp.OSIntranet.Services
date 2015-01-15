using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Functionality which handles the query for getting a collection of translation informations.
    /// </summary>
    public class TranslationInfoCollectionGetQueryHandler : IQueryHandler<TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>
    {
        #region Private variables

        private readonly ISystemDataRepository _systemDataRepository;
        private readonly IFoodWasteObjectMapper _foodWasteObjectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates functionality which handles the query for getting a collection of translation informations.
        /// </summary>
        /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
        public TranslationInfoCollectionGetQueryHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper)
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
        /// Functionality which handles the query for getting a collection of translation informations.
        /// </summary>
        /// <param name="query">Query for getting a collection of translation informations.</param>
        /// <returns>Collection of translation informations.</returns>
        public virtual IEnumerable<TranslationInfoSystemView> Query(TranslationInfoCollectionGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var translationInfoCollection = _systemDataRepository.TranslationInfoGetAll();
            return _foodWasteObjectMapper.Map<IEnumerable<ITranslationInfo>, IEnumerable<TranslationInfoSystemView>>(translationInfoCollection.Where(m => m.Identifier.HasValue));
        }

        #endregion
    }
}
