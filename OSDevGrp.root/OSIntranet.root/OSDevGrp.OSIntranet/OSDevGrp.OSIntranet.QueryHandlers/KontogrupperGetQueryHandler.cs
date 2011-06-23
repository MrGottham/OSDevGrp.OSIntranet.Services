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
    /// QueryHandler til håndtering af forespørgelsen: KontogrupperGetQuery.
    /// </summary>
    public class KontogrupperGetQueryHandler : IQueryHandler<KontogrupperGetQuery, IEnumerable<KontogruppeView>>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: KontogrupperGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public KontogrupperGetQueryHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
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

        #region IQueryHandler<KontogrupperGetQuery,IEnumerable<KontogruppeView>> Members

        /// <summary>
        /// Henter og returnerer kontogrupper.
        /// </summary>
        /// <param name="query">Forespørgelse efter kontogrupper.</param>
        /// <returns>List af kontogrupper.</returns>
        public IEnumerable<KontogruppeView> Query(KontogrupperGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var kontogrupper = _finansstyringRepository.KontogruppeGetAll();
            return _objectMapper.Map<IEnumerable<Kontogruppe>, IEnumerable<KontogruppeView>>(kontogrupper);
        }

        #endregion
    }
}
