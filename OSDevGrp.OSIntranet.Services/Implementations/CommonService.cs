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
    /// Service til fælles elementer.
    /// </summary>
    public class CommonService : IntranetServiceBase, ICommonService
    {
        #region Private variables

        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner service til fælles elementer.
        /// </summary>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public CommonService(IQueryBus queryBus)
        {
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            _queryBus = queryBus;
        }

        #endregion

        #region ICommonService Members

        /// <summary>
        /// Henter alle systemer under OSWEBDB.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle systemer under OSWEBDB.</param>
        /// <returns>Liste af alle systemer under OSWEBDB.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<SystemView> SystemerGet(SystemerGetQuery query)
        {
            try
            {
                return _queryBus.Query<SystemerGetQuery, IEnumerable<SystemView>>(query);
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