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
    /// QueryHandler til håndtering af forespørgelsen: AdressekontolisteGetQuery.
    /// </summary>
    public class AdressekontolisteGetQueryHandler :AdressekontoQueryHandlerBase, IQueryHandler<AdressekontolisteGetQuery, IEnumerable<AdressekontolisteView>>
    {
        #region Private variables

        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: AdressekontolisteGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public AdressekontolisteGetQueryHandler(IAdresseRepository adresseRepository, IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
            : base(adresseRepository, finansstyringRepository)
        {
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _objectMapper = objectMapper;
        }

        #endregion

        #region IQueryHandler<AdressekontolisteGetQuery,IEnumerable<AdressekontolisteView>> Members

        /// <summary>
        /// Henter og returnerer en liste af adressekonti.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af adressekonti.</param>
        /// <returns>Liste af adressekonti.</returns>
        public IEnumerable<AdressekontolisteView> Query(AdressekontolisteGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var adressekonti = AdressekontoGetAllByRegnskabsnummer(query.Regnskabsnummer, query.StatusDato);
            return _objectMapper.Map<IEnumerable<AdresseBase>, IEnumerable<AdressekontolisteView>>(adressekonti);
        }

        #endregion
    }
}
