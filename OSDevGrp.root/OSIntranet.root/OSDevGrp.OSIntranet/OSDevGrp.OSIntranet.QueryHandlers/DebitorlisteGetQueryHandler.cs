using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: DebitorlisteGetQuery.
    /// </summary>
    public class DebitorlisteGetQueryHandler : RegnskabQueryHandlerBase, IQueryHandler<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>
    {
        #region Private variables

        private readonly IKonfigurationRepository _konfigurationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: DebitorlisteGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="konfigurationRepository">Implementering af konfigurationsrepository.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public DebitorlisteGetQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IKonfigurationRepository konfigurationRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper)
        {
            if (konfigurationRepository == null)
            {
                throw new ArgumentNullException("konfigurationRepository");
            }
            _konfigurationRepository = konfigurationRepository;
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

            var overNul = _konfigurationRepository.DebitorSaldoOverNul;
            var debitorer = AdressekontoGetAllWithValueByRegnskabAndStatusDato(query.Regnskabsnummer, query.StatusDato,
                                                                               overNul);

            return MapMany<AdresseBase, DebitorlisteView>(debitorer);
        }

        #endregion
    }
}
