using System;
using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Services.Implementations
{
    /// <summary>
    /// Service til finansstyring.
    /// </summary>
    public class FinansstyringService : IntranetServiceBase, IFinansstyringService
    {
        #region Private variables

        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner service til finansstyring.
        /// </summary>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public FinansstyringService(IQueryBus queryBus)
        {
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            _queryBus = queryBus;
        }

        #endregion

        #region IFinansstyringService Members

        /// <summary>
        /// Henter en regnskabsliste.
        /// </summary>
        /// <param name="query">Forespørgelse efter en regnskabsliste.</param>
        /// <returns>Regnskabsliste.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<RegnskabslisteView> RegnskabslisteGet(RegnskabslisteGetQuery query)
        {
            try
            {
                return _queryBus.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(query);
            }
            catch (IntranetRepositoryException ex)
            {
                throw CreateIntranetRepositoryFault(ex);
            }
            catch (IntranetBusinessException ex)
            {
                throw CreateIntranetBusinessFault(ex);
            }
            catch (IntranetSystemException ex)
            {
                throw CreateIntranetSystemFault(ex);
            }
            catch (Exception ex)
            {
                throw CreateIntranetSystemFault(ex);
            }
        }

        #endregion
    }
}