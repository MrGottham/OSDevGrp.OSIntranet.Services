using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.QueryHandlers.Core
{
    /// <summary>
    /// Functionality which handles a query for getting a collection of storage types.
    /// </summary>
    /// <typeparam name="TQuery">Type of the query for getting a collection of storage types.</typeparam>
    /// <typeparam name="TView">Type of the view to return for the storages types.</typeparam>
    public abstract class StorageTypeCollectionGetQueryHandlerBase<TQuery, TView> : IQueryHandler<TQuery, IEnumerable<TView>> where TQuery : StorageTypeCollectionGetQuery where TView : StorageTypeIdentificationView
    {
        #region Private variables

        private readonly ISystemDataRepository _systemDataRepository;
        private readonly IFoodWasteObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the functionality which handles a query for getting a collection of storage types.
        /// </summary>
        /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
        /// <param name="objectMapper">Implementation of the object mapper which can map objects in the food waste domain.</param>
        protected StorageTypeCollectionGetQueryHandlerBase(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper objectMapper)
        {
            _systemDataRepository = systemDataRepository ?? throw new ArgumentNullException(nameof(systemDataRepository));
            _objectMapper = objectMapper ?? throw new ArgumentNullException(nameof(objectMapper));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Functionality which handles a query for getting a collection of storage types.
        /// </summary>
        /// <param name="query">Query for getting a collection of storage types.</param>
        /// <returns>Collection of the storages types.</returns>
        public virtual IEnumerable<TView> Query(TQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            ITranslationInfo translationInfo = _systemDataRepository.Get<ITranslationInfo>(query.TranslationInfoIdentifier);
            IEnumerable<IStorageType> storageTypeCollection = _systemDataRepository.StorageTypeGetAll();

            return _objectMapper.Map<IEnumerable<IStorageType>, IEnumerable<TView>>(storageTypeCollection, translationInfo.CultureInfo);
        }

        #endregion
    }
}
