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
    /// QueryHandler til håndtering af forespørgelsen: FirmaGetQuery.
    /// </summary>
    public class FirmaGetQueryHandler : AdressekartotekQueryHandlerBase, IQueryHandler<FirmaGetQuery, FirmaView>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: FirmaGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public FirmaGetQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
            : base(adresseRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<FirmaGetQuery,FirmaView> Members

        /// <summary>
        /// Henter og returnerer et givent firma.
        /// </summary>
        /// <param name="query">Forespørgelse efter et givent firma.</param>
        /// <returns>Firma.</returns>
        public FirmaView Query(FirmaGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var firma = FirmaGetByNummer(query.Nummer);

            return Map<Firma, FirmaView>(firma);
        }

        #endregion
    }
}
