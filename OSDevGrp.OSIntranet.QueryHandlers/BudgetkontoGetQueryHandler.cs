using System;
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
    /// QueryHandler til håndtering af forespørgelsen: BudgetkontoGetQuery.
    /// </summary>
    public class BudgetkontoGetQueryHandler : IQueryHandler<BudgetkontoGetQuery, BudgetkontoView> 
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: BudgetkontoGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BudgetkontoGetQueryHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
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

        #region IQueryHandler<BudgetkontoGetQuery,BudgetkontoView> Members

        /// <summary>
        /// Henter og returnerer en budgetkonto.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en budgetkonto.</param>
        /// <returns>Budgetkonto.</returns>
        public BudgetkontoView Query(BudgetkontoGetQuery query)
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
            if (string.IsNullOrEmpty(query.Kontonummer))
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue,
                                                                               query.Kontonummer, "query.Kontonummer"));
            }
            Budgetkonto budgetkonto;
            try
            {
                budgetkonto = regnskab.Konti
                    .OfType<Budgetkonto>().Single(m => m.Kontonummer.CompareTo(query.Kontonummer) == 0);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Budgetkonto),
                                                 query.Kontonummer), ex);
            }
            return _objectMapper.Map<Budgetkonto, BudgetkontoView>(budgetkonto);
        }

        #endregion
    }
}
