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
    /// QueryHandler til håndtering af forespørgelsen: AdressekontoGetQuery.
    /// </summary>
    public class AdressekontoGetQueryHandler : AdressekontoQueryHandlerBase, IQueryHandler<AdressekontoGetQuery, AdressekontoView>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: AdressekontoGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public AdressekontoGetQueryHandler(IAdresseRepository adresseRepository, IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
            : base(adresseRepository, finansstyringRepository)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
