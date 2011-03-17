using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: DebitorlisteGetQuery.
    /// </summary>
    public class DebitorlisteGetQueryHandler : AdressekontoQueryHandlerBase, IQueryHandler<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>
    {
        #region Private variables

        private readonly IKonfigurationRepository _konfigurationRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: DebitorlisteGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="konfigurationRepository">Implementering af konfigurationsrepository.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public DebitorlisteGetQueryHandler(IAdresseRepository adresseRepository, IFinansstyringRepository finansstyringRepository, IKonfigurationRepository konfigurationRepository, IObjectMapper objectMapper)
            : base(adresseRepository, finansstyringRepository)
        {
            if (konfigurationRepository == null)
            {
                throw new ArgumentNullException("konfigurationRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _konfigurationRepository = konfigurationRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region IQueryHandler<DebitorlisteGetQuery,IEnumerable<DebitorlisteView>> Members

        /// <summary>
        /// Henter og returnerer en debitorliste.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en debitorliste.</param>
        /// <returns>Debitorliste.</returns>
        public IEnumerable<DebitorlisteView> Query(DebitorlisteGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var debitorer = AdressekontoGetAllWithValueByRegnskabsnummer(query.Regnskabsnummer, query.StatusDato,
                                                                         _konfigurationRepository.DebitorSaldoOverNul);
            return _objectMapper.Map<IEnumerable<AdresseBase>, IEnumerable<DebitorlisteView>>(debitorer);
        }

        #endregion
    }
}
