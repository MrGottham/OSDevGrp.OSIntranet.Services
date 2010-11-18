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
    /// QueryHandler til håndtering af forespørgelsen: KontoplanGetQuery.
    /// </summary>
    public class KontoplanGetQueryHandler : IQueryHandler<KontoplanGetQuery, IEnumerable<KontoplanView>>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: KontoplanGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public KontoplanGetQueryHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
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

        #region IQueryHandler<KontoplanGetQuery,IEnumerable<KontoplanView>> Members

        /// <summary>
        /// Henter og returnerer en kontoplan.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en kontoplan.</param>
        /// <returns>Kontoplan.</returns>
        public IEnumerable<KontoplanView> Query(KontoplanGetQuery query)
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
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
