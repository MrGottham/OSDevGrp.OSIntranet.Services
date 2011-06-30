using System;
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
    /// QueryHandler til håndtering af forespørgelsen: AdressekontoGetQuery.
    /// </summary>
    public class AdressekontoGetQueryHandler : RegnskabQueryHandlerBase, IQueryHandler<AdressekontoGetQuery, AdressekontoView>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: AdressekontoGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public AdressekontoGetQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<AdressekontoGetQuery,AdressekontoView> Members

        /// <summary>
        /// Henter og returnerer en given adressekonto.
        /// </summary>
        /// <param name="query">Forespørgelse efter en given adressekonto.</param>
        /// <returns>Adressekonto.</returns>
        public AdressekontoView Query(AdressekontoGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            
            var adressekonto = AdressekontoGetByRegnskabAndNummer(query.Regnskabsnummer, query.Nummer);
            adressekonto.Calculate(query.StatusDato);

            return Map<AdresseBase, AdressekontoView>(adressekonto);
        }

        #endregion
    }
}
