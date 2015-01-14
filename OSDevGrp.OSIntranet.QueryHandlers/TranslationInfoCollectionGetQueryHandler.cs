using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Functionality which handles the query for getting a collection of translation informations.
    /// </summary>
    public class TranslationInfoCollectionGetQueryHandler : IQueryHandler<TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>
    {
        #region Methods

        /// <summary>
        /// Functionality which handles the query for getting a collection of translation informations.
        /// </summary>
        /// <param name="query">Query for getting a collection of translation informations.</param>
        /// <returns>Collection of translation informations.</returns>
        public virtual IEnumerable<TranslationInfoSystemView> Query(TranslationInfoCollectionGetQuery query)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
