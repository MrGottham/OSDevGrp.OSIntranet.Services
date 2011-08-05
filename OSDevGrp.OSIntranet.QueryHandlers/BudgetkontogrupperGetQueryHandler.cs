using System;
using System.Collections.Generic;
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
    /// QueryHandler til håndtering af forespørgelsen: BudgetkontogrupperGetQuery.
    /// </summary>
    public class BudgetkontogrupperGetQueryHandler : FinansstyringQueryHandlerBase, IQueryHandler<BudgetkontogrupperGetQuery, IEnumerable<BudgetkontogruppeView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: BudgetkontogrupperGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BudgetkontogrupperGetQueryHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<BudgetkontogrupperGetQuery,IEnumerable<BudgetkontogruppeView>> Members

        /// <summary>
        /// Henter og returnerer grupper til budgetkonti..
        /// </summary>
        /// <param name="query">Forespørgelse efter grupper til budgetkonti.</param>
        /// <returns>Liste af grupper til budgetkonti.</returns>
        public IEnumerable<BudgetkontogruppeView> Query(BudgetkontogrupperGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            
            var budgetkontogrupper = Repository.BudgetkontogruppeGetAll();

            return MapMany<Budgetkontogruppe, BudgetkontogruppeView>(budgetkontogrupper);
        }

        #endregion
    }
}
