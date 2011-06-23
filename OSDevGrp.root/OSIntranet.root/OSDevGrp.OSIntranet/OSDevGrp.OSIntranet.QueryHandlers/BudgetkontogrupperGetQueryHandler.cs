using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: BudgetkontogrupperGetQuery.
    /// </summary>
    public class BudgetkontogrupperGetQueryHandler : IQueryHandler<BudgetkontogrupperGetQuery, IEnumerable<BudgetkontogruppeView>>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: BudgetkontogrupperGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BudgetkontogrupperGetQueryHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
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
            var budgetkontogrupper = _finansstyringRepository.BudgetkontogruppeGetAll();
            return
                _objectMapper.Map<IEnumerable<Budgetkontogruppe>, IEnumerable<BudgetkontogruppeView>>(budgetkontogrupper);
        }

        #endregion
    }
}
