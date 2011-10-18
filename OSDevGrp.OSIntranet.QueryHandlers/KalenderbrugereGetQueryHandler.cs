using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: KalenderbrugereGetQuery.
    /// </summary>
    public class KalenderbrugereGetQueryHandler : KalenderQueryHandlerBase, IQueryHandler<KalenderbrugereGetQuery, IEnumerable<KalenderbrugerView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: KalenderbrugereGetQuery.
        /// </summary>
        /// <param name="kalenderRepositor">Implementering af repository til kalenderdelen under OSWEBDB.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet, såsom systemer under OSWEBDB.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public KalenderbrugereGetQueryHandler(IKalenderRepository kalenderRepositor, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(kalenderRepositor, fællesRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<KalenderbrugereGetQuery,IEnumerable<KalenderbrugerView>> Members

        /// <summary>
        /// Henter og returnerer kalenderbrugere til et givent system under OSWEBDB.
        /// </summary>
        /// <param name="query">Foresprøgelse efter kalenderbrugere til et givent system under OSWEBDB.</param>
        /// <returns>Liste indeholdende kalenderbrugere til systemet under OSWEBDB.</returns>
        public IEnumerable<KalenderbrugerView> Query(KalenderbrugereGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var system = SystemGetByNummer(query.System);
            var brugere = KalenderRepository.BrugerGetAllBySystem(system.Nummer);

            return MapMany<IBruger, KalenderbrugerView>(brugere);
        }

        #endregion
    }
}
