using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Comparers;
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
    /// QueryHandler til håndtering af forespørgelsen: BogføringerGetQuery.
    /// </summary>
    public class BogføringerGetQueryHandler : RegnskabQueryHandlerBase, IQueryHandler<BogføringerGetQuery, IEnumerable<BogføringslinjeView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: BogføringerGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BogføringerGetQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<BogføringerGetQuery,IEnumerable<BogføringslinjeView>> Members

        /// <summary>
        /// Henter og returnerer et givent antal bogføringslinjer fra en given statusdato.
        /// </summary>
        /// <param name="query">Forespørgelse efter et givent antal bogføringslinjer.</param>
        /// <returns>Liste af bogføringslinjer.</returns>
        public IEnumerable<BogføringslinjeView> Query(BogføringerGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var konti = KontoGetAllByRegnskab(query.Regnskabsnummer);

            var bogføringslinjeComparer = new BogføringslinjeComparer();
            var bogføringslinjer = konti.OfType<Konto>()
                .SelectMany(m => m.Bogføringslinjer)
                .OrderByDescending(m => m, bogføringslinjeComparer)
                .Where(m => m.Dato.CompareTo(query.StatusDato) <= 0)
                .Take(query.Linjer)
                .ToList();

            return bogføringslinjer.Select(Map<Bogføringslinje, BogføringslinjeView>).ToList();
        }

        #endregion
    }
}
