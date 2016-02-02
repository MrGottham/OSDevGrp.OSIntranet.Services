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
    /// Query handler which handles the query for getting a collection of data providers who handles payments.
    /// </summary>
    public class DataProviderWhoHandlesPaymentsCollectionGetQueryHandler : IQueryHandler<DataProviderWhoHandlesPaymentsCollectionGetQuery, IEnumerable<DataProviderView>>
    {
        #region Private variables

        private readonly ISystemDataRepository _systemDataRepository;
        private readonly IFoodWasteObjectMapper _foodWasteObjectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a query handler which handles the query for getting a collection of data providers who handles payments.
        /// </summary>
        /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
        public DataProviderWhoHandlesPaymentsCollectionGetQueryHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper)
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
        /// Functionality which handles the query for getting a collection of data providers who handles payments.
        /// </summary>
        /// <param name="query">Query for getting a collection of data providers who handles payments.</param>
        /// <returns>Collection of data providers who handles payments.</returns>
        public virtual IEnumerable<DataProviderView> Query(DataProviderWhoHandlesPaymentsCollectionGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var translationInfo = _systemDataRepository.Get<ITranslationInfo>(query.TranslationInfoIdentifier);
            var dataProviders = _systemDataRepository.DataProviderWhoHandlesPaymentsGetAll();

            return _foodWasteObjectMapper.Map<IEnumerable<IDataProvider>, IEnumerable<DataProviderView>>(dataProviders, translationInfo.CultureInfo);
        }

        #endregion
    }
}
