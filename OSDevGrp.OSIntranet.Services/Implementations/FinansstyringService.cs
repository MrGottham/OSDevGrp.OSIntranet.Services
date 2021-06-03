using System;
using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Responses;
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

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner service til finansstyring.
        /// </summary>
        /// <param name="commandBus">Implementering af en CommandBus.</param>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public FinansstyringService(ICommandBus commandBus, IQueryBus queryBus)
        {
            if (commandBus == null)
            {
                throw new ArgumentNullException("commandBus");
            }
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        #endregion

        #region IFinansstyringService Members

        /// <summary>
        /// Henter en kontoplan.
        /// </summary>
        /// <param name="query">Forespørgelse efter en kontoplan.</param>
        /// <returns>Kontoplan.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<KontoplanView> KontoplanGet(KontoplanGetQuery query)
        {
            try
            {
                return _queryBus.Query<KontoplanGetQuery, IEnumerable<KontoplanView>>(query);
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
        /// Henter en konto.
        /// </summary>
        /// <param name="query">Forespørgelse efter en konto.</param>
        /// <returns>Konto.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public KontoView KontoGet(KontoGetQuery query)
        {
            try
            {
                return _queryBus.Query<KontoGetQuery, KontoView>(query);
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
        /// Henter en budgetkontoplan.
        /// </summary>
        /// <param name="query">Forespørgelse efter en budgetkontoplan.</param>
        /// <returns>Budgetkontoplan.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<BudgetkontoplanView> BudgetkontoplanGet(BudgetkontoplanGetQuery query)
        {
            try
            {
                return _queryBus.Query<BudgetkontoplanGetQuery, IEnumerable<BudgetkontoplanView>>(query);
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
        /// Henter en budgetkonto.
        /// </summary>
        /// <param name="query">Forespørgelse efter en budgetkonto.</param>
        /// <returns>Budgetkonto.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public BudgetkontoView BudgetkontoGet(BudgetkontoGetQuery query)
        {
            try
            {
                return _queryBus.Query<BudgetkontoGetQuery, BudgetkontoView>(query);
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
        /// Henter et givent antal bogføringslinjer fra en given statusdato.
        /// </summary>
        /// <param name="query">Forespørgelse efter et givent antal bogføringslinjer.</param>
        /// <returns>Liste af bogføringslinjer.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<BogføringslinjeView> BogføringerGet(BogføringerGetQuery query)
        {
            try
            {
                return _queryBus.Query<BogføringerGetQuery, IEnumerable<BogføringslinjeView>>(query);
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
        /// Opretter en bogføringslinje.
        /// </summary>
        /// <param name="command">Kommando til oprettelse af en bogføringslinje.</param>
        /// <returns>Svar fra oprettelse af en bogføringslinje.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public BogføringslinjeOpretResponse BogføringslinjeOpret(BogføringslinjeOpretCommand command)
        {
            try
            {
                return _commandBus.Publish<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(command);
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