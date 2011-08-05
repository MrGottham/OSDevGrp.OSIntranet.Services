using System;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: KontoGetQuery.
    /// </summary>
    public class KontoGetQueryHandler : RegnskabQueryHandlerBase, IQueryHandler<KontoGetQuery, KontoView>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: KontoGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public KontoGetQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<KontoGetQuery,KontoView> Members

        /// <summary>
        /// Henter og returnerer en konto.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en konto.</param>
        /// <returns>Konto.</returns>
        public KontoView Query(KontoGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var konto = KontoGetByRegnskabAndKontonummer(query.Regnskabsnummer, query.Kontonummer);
            konto.Calculate(query.StatusDato);

            return Map<Konto, KontoView>(konto);
        }

        #endregion
    }
}
