using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Services.Implementations
{
    /// <summary>
    /// Service til finansstyring.
    /// </summary>
    public class FinansstyringService : IntranetServiceBase, IFinansstyringService
    {
        #region IFinansstyringService Members

        /// <summary>
        /// Henter en regnskabsliste.
        /// </summary>
        /// <param name="getQuery">Forespørgelse efter en regnskabsliste.</param>
        /// <returns>Regnskabsliste.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<RegnskabslisteView> RegnskabslisteGet(RegnskabslisteGetQuery getQuery)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}