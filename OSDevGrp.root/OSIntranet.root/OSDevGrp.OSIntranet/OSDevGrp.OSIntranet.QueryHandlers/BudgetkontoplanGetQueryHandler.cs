using System;
using System.Collections.Generic;
using System.Linq;
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
    /// QueryHandler til håndtering af forespørgelsen: BudgetkontoplanGetQuery.
    /// </summary>
    public class BudgetkontoplanGetQueryHandler : RegnskabQueryHandlerBase, IQueryHandler<BudgetkontoplanGetQuery, IEnumerable<BudgetkontoplanView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: BudgetkontoplanGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BudgetkontoplanGetQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper)
        {
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

            var budgetkonti = BudgetkontoGetAllByRegnskab(query.Regnskabsnummer).ToList();
            budgetkonti.ForEach(m => m.Calculate(query.StatusDato));

            return MapMany<Budgetkonto, BudgetkontoplanView>(budgetkonti);
        }

        #endregion
    }
}
