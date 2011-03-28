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
        /// Henter en liste af debitorer.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af debitorer.</param>
        /// <returns>Liste af debitorer.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<DebitorlisteView> DebitorlisteGet(DebitorlisteGetQuery query)
        {
            try
            {
                return _queryBus.Query<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>(query);
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
        /// Henter en debitor.
        /// </summary>
        /// <param name="query">Forespørgelse efter en debitor.</param>
        /// <returns>Debitor.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public DebitorView DebitorGet(DebitorGetQuery query)
        {
            try
            {
                return _queryBus.Query<DebitorGetQuery, DebitorView>(query);
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
        /// Henter en liste af kreditorer.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af kreditorer.</param>
        /// <returns>Liste af kreditorer.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<KreditorlisteView> KreditorlisteGet(KreditorlisteGetQuery query)
        {
            try
            {
                return _queryBus.Query<KreditorlisteGetQuery, IEnumerable<KreditorlisteView>>(query);
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
        /// Henter en kreditor.
        /// </summary>
        /// <param name="query">Forespørgelse efter en kreditor.</param>
        /// <returns>Kreditor</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public KreditorView KreditorGet(KreditorGetQuery query)
        {
            try
            {
                return _queryBus.Query<KreditorGetQuery, KreditorView>(query);
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
        /// Hente en liste af adressekonti.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af adressekonti.</param>
        /// <returns>Liste af adressekonti.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<AdressekontolisteView> AdressekontolisteGet(AdressekontolisteGetQuery query)
        {
            try
            {
                return _queryBus.Query<AdressekontolisteGetQuery, IEnumerable<AdressekontolisteView>>(query);
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
        /// Hente en adressekonto.
        /// </summary>
        /// <param name="query">Forespørgelse efter en adressekonto.</param>
        /// <returns>Liste af adressekonto.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public AdressekontoView AdressekontoGet(AdressekontoGetQuery query)
        {
            try
            {
                return _queryBus.Query<AdressekontoGetQuery, AdressekontoView>(query);
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <param name="query">Foresprøgelse efter alle betalingsbetingelser.</param>
        /// <returns>Liste af betalingsbetingelser.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<BetalingsbetingelseView> BetalingsbetingelserGet(BetalingsbetingelserGetQuery query)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter alle kontogrupper.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle kontogrupper.</param>
        /// <returns>Liste af kontogrupper.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<KontogruppeView> KontogrupperGet(KontogrupperGetQuery query)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter alle grupper af budgetkonti.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle grupper af budgetkonti.</param>
        /// <returns>Liste af grupper for budgetkonti.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<BudgetkontogruppeView> BudgetkontogrupperGet(BudgetkontogrupperGetQuery query)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}