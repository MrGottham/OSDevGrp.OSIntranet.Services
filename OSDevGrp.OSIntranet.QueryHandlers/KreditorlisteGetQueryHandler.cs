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
    /// QueryHandler til håndtering af forespørgelsen: KreditorlisteGetQuery.
    /// </summary>
    public class KreditorlisteGetQueryHandler : AdressekontoQueryHandlerBase, IQueryHandler<KreditorlisteGetQuery, IEnumerable<KreditorlisteView>>
    {
        #region Private variables

        private readonly IKonfigurationRepository _konfigurationRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: KreditorlisteGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="konfigurationRepository">Implementering af konfigurationsrepository.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public KreditorlisteGetQueryHandler(IAdresseRepository adresseRepository, IFinansstyringRepository finansstyringRepository, IKonfigurationRepository konfigurationRepository, IObjectMapper objectMapper)
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

        #region IQueryHandler<KreditorlisteGetQuery,IEnumerable<KreditorlisteView>> Members

        /// <summary>
        /// Henter og returnerer en kreditorliste.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en kreditorliste.</param>
        /// <returns>Kreditorliste</returns>
        public IEnumerable<KreditorlisteView> Query(KreditorlisteGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}
