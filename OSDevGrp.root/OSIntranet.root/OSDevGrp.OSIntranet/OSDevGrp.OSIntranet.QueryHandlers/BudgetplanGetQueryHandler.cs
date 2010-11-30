using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: BudgetplanGetQuery.
    /// </summary>
    public class BudgetplanGetQueryHandler : IQueryHandler<BudgetplanGetQuery, IEnumerable<BudgetplanView>>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: BudgetplanGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BudgetplanGetQueryHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
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

        #region IQueryHandler<BudgetplanGetQuery,IEnumerable<BudgetplanView>> Members

        /// <summary>
        /// Henter og returnerer en budgetplan.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en kontoplan.</param>
        /// <returns>Budgetplan.</returns>
        public IEnumerable<BudgetplanView> Query(BudgetplanGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            if (query.Budgethistorik <= 0)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue,
                                                                               query.Budgethistorik, "Budgethistorik"));
            }
            var regnskab = _finansstyringRepository.RegnskabGet(query.Regnskabsnummer);
            foreach (var calculatable in regnskab.Konti.OfType<ICalculatable>())
            {
                calculatable.Calculate(query.StatusDato);
            }
            var calculateFrom = query.StatusDato.AddMonths((query.Budgethistorik - 1)*-1);
            var budgetkontoplanViews = _objectMapper
                .Map<IList<Budgetkonto>, IEnumerable<BudgetkontoplanView>>(
                    regnskab.Konti.OfType<Budgetkonto>().ToList());
            foreach (var budgetkontoplanView in budgetkontoplanViews)
            {
                budgetkontoplanView.Budgetoplysninger = budgetkontoplanView.Budgetoplysninger
                    .Where(
                        m =>
                        ((m.År > calculateFrom.Year) || (m.År == calculateFrom.Year && m.Måned >= calculateFrom.Month)) &&
                        ((m.År < query.StatusDato.Year) ||
                         (m.År == query.StatusDato.Year && m.Måned <= query.StatusDato.Month)))
                    .ToList();
            }
            var budgetplanViews = _objectMapper
                .Map<IList<Budgetkontogruppe>, IEnumerable<BudgetplanView>>(
                    _finansstyringRepository.BudgetkontogruppeGetAll());
            throw new NotImplementedException();
        }

        #endregion
    }
}
