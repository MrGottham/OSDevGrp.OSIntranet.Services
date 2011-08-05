using System;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: BudgetkontoGetQuery.
    /// </summary>
    public class BudgetkontoGetQueryHandler : RegnskabQueryHandlerBase, IQueryHandler<BudgetkontoGetQuery, BudgetkontoView> 
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: BudgetkontoGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BudgetkontoGetQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper)
        {
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

            var budgetkonto = BudgetkontoGetByRegnskabAndKontonummer(query.Regnskabsnummer, query.Kontonummer);
            budgetkonto.Calculate(query.StatusDato);

            return Map<Budgetkonto, BudgetkontoView>(budgetkonto);
        }

        #endregion
    }
}
