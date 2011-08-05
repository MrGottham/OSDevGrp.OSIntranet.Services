using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: KontoplanGetQuery.
    /// </summary>
    public class KontoplanGetQueryHandler : RegnskabQueryHandlerBase, IQueryHandler<KontoplanGetQuery, IEnumerable<KontoplanView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: KontoplanGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public KontoplanGetQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<KontoplanGetQuery,IEnumerable<KontoplanView>> Members

        /// <summary>
        /// Henter og returnerer en kontoplan.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en kontoplan.</param>
        /// <returns>Kontoplan.</returns>
        public IEnumerable<KontoplanView> Query(KontoplanGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var konti = KontoGetAllByRegnskab(query.Regnskabsnummer).ToList();
            konti.ForEach(m => m.Calculate(query.StatusDato));

            return MapMany<Konto, KontoplanView>(konti);
        }

        #endregion
    }
}
