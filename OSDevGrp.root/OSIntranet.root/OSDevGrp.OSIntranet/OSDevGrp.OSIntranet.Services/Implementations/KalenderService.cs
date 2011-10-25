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
    /// Service til kalender.
    /// </summary>
    public class KalenderService : IntranetServiceBase, IKalenderService
    {
        #region Private variables

        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner service til kalender.
        /// </summary>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public KalenderService(IQueryBus queryBus)
        {
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            _queryBus = queryBus;
        }

        #endregion

        #region IKalenderService Members

        /// <summary>
        /// Henter kalenderaftaler til en given kalenderbruger.
        /// </summary>
        /// <param name="query">Forespørgelse efter kalenderaftaler til en given kalenderbruger.</param>
        /// <returns>Liste af kalenderaftaler til den givne kalenderbruger.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<KalenderbrugerAftaleView> KalenderbrugerAftalerGet(KalenderbrugerAftalerGetQuery query)
        {
            try
            {
                return _queryBus.Query<KalenderbrugerAftalerGetQuery, IEnumerable<KalenderbrugerAftaleView>>(query);
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

        /// <summary>
        /// Henter en given kalenderaftale til en given kalenderbruger.
        /// </summary>
        /// <param name="query">Forespørgelse efter en given kalenderaftale til en given kalenderbruger.</param>
        /// <returns>Kalenderaftale til den givne givne kalenderbruger.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public KalenderbrugerAftaleView KalenderbrugerAftaleGet(KalenderbrugerAftaleGetQuery query)
        {
            try
            {
                return _queryBus.Query<KalenderbrugerAftaleGetQuery, KalenderbrugerAftaleView>(query);
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

        /// <summary>
        /// Henter alle kalenderbrugere til et givent system under OSWEBDB.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle kalenderbrugere til et givent system under OSWEBDB.</param>
        /// <returns>Liste af alle kalenderbrugere til det givne system under OSWEBDB.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<KalenderbrugerView> KalenderbrugereGet(KalenderbrugereGetQuery query)
        {
            try
            {
                return _queryBus.Query<KalenderbrugereGetQuery, IEnumerable<KalenderbrugerView>>(query);
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