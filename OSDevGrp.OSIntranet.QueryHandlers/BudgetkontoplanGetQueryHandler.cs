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
    public class BudgetkontoplanGetQueryHandler : IQueryHandler<BudgetkontoplanGetQuery, IEnumerable<BudgetkontoplanView>>
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

        #region IQueryHandler<BudgetkontoplanGetQuery,IEnumerable<BudgetkontoplanView>> Members

        /// <summary>
        /// Henter og returnerer en budgetkontoplan.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en kontokontoplan.</param>
        /// <returns>Budgetkontoplan.</returns>
        public IEnumerable<BudgetkontoplanView> Query(BudgetkontoplanGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var regnskab = _finansstyringRepository.RegnskabGet(query.Regnskabsnummer);
            foreach (var calculatable in regnskab.Konti.OfType<ICalculatable>())
            {
                calculatable.Calculate(query.StatusDato);
            }
            return
                _objectMapper.Map<IList<Budgetkonto>, IEnumerable<BudgetkontoplanView>>(
                    regnskab.Konti.OfType<Budgetkonto>().ToList());
        }

        #endregion
    }
}
