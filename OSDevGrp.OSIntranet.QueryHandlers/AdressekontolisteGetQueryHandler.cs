using System;
using System.Collections.Generic;
using System.Linq;
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
    /// QueryHandler til håndtering af forespørgelsen: AdressekontolisteGetQuery.
    /// </summary>
    public class AdressekontolisteGetQueryHandler : RegnskabQueryHandlerBase, IQueryHandler<AdressekontolisteGetQuery, IEnumerable<AdressekontolisteView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: AdressekontolisteGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public AdressekontolisteGetQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper)
        {
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

            var adressekonti = AdressekontoGetAllByRegnskab(query.Regnskabsnummer).ToList();
            adressekonti.ForEach(m => m.Calculate(query.StatusDato));

            return MapMany<AdresseBase, AdressekontolisteView>(adressekonti);
        }

        #endregion
    }
}
