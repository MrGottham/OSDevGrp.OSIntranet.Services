using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: BudgetkontoplanGetQuery.
    /// </summary>
    public class BudgetkontoplanGetQueryHandler : IQueryHandler<BudgetkontoplanGetQuery, IEnumerable<BudgetplanView>>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: BudgetkontoplanGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BudgetkontoplanGetQueryHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _finansstyringRepository = finansstyringRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region IQueryHandler<BudgetkontoplanGetQuery,IEnumerable<BudgetplanView>> Members

        /// <summary>
        /// Henter og returnerer en budgetplan.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en kontoplan.</param>
        /// <returns>Budgetplan.</returns>
        public IEnumerable<BudgetplanView> Query(BudgetkontoplanGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            // Beregning af budgetkonti i forhold til statusdato.
            var regnskab = _finansstyringRepository.RegnskabGet(query.Regnskabsnummer);
            foreach (var calculatable in regnskab.Konti.OfType<ICalculatable>())
            {
                calculatable.Calculate(query.StatusDato);
            }
            // Dan liste af budgetkonti med budgetoplysninger for de ønskede måneder.
            var budgetkontoplanViews = _objectMapper
                .Map<IList<Budgetkonto>, IEnumerable<BudgetkontoplanView>>(
                    regnskab.Konti.OfType<Budgetkonto>().ToList());
            // Dan liste af budgetkontogrupper med budgetkonti og budgetoplysninger for de ønskede måneder.
            var budgetplanViews = _objectMapper
                .Map<IList<Budgetkontogruppe>, IEnumerable<BudgetplanView>>(
                    _finansstyringRepository.BudgetkontogruppeGetAll());
            foreach (var budgetplanView in budgetplanViews)
            {
                var view = budgetplanView;
                budgetplanView.Budgetkonti = budgetkontoplanViews
                    .Where(m => m.Budgetkontogruppe.Nummer == view.Nummer)
                    .ToList();
                view.Budget = 0M;
                view.Bogført = 0M;
                view.Disponibel = 0M;
            }
            return budgetplanViews;
        }

        #endregion
    }
}
