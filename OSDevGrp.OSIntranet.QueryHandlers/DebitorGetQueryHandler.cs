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
    /// /// QueryHandler til håndtering af forespørgelsen: DebitorGetQuery.
    /// </summary>
    public class DebitorGetQueryHandler : RegnskabQueryHandlerBase, IQueryHandler<DebitorGetQuery, DebitorView>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: DebitorGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public DebitorGetQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<DebitorGetQuery,DebitorView> Members

        /// <summary>
        /// Henter og returnerer en debitor.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en debitor.</param>
        /// <returns>Debitor.</returns>
        public DebitorView Query(DebitorGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var adressekonto = AdressekontoGetByRegnskabAndNummer(query.Regnskabsnummer, query.Nummer);
            adressekonto.Calculate(query.StatusDato);

            return Map<AdresseBase, DebitorView>(adressekonto);
        }

        #endregion
    }
}
