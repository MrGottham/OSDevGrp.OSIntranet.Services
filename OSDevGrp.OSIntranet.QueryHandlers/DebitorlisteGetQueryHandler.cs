using System;
using System.Collections.Generic;
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
    public class DebitorlisteGetQueryHandler : IQueryHandler<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IFinansstyringRepository _finansstyringRepository;
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
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _adresseRepository = adresseRepository;
            _finansstyringRepository = finansstyringRepository;
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
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
