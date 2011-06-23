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
    /// QueryHandler til håndtering af forespørgelsen: RegnskabslisteGetQuery.
    /// </summary>
    public class RegnskabslisteGetQueryHandler : IQueryHandler<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: RegnskabslisteGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public RegnskabslisteGetQueryHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
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

        #region IQueryHandler<RegnskabslisteGetQuery,IEnumerable<RegnskabslisteView>> Members

        /// <summary>
        /// Henter og returnerer en regnskabsliste.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en regnskabsliste.</param>
        /// <returns>Regnskabsliste.</returns>
        public IEnumerable<RegnskabslisteView> Query(RegnskabslisteGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var regnskaber = _finansstyringRepository.RegnskabslisteGet();
            return _objectMapper.Map<IEnumerable<Regnskab>, IEnumerable<RegnskabslisteView>>(regnskaber);
        }

        #endregion
    }
}
