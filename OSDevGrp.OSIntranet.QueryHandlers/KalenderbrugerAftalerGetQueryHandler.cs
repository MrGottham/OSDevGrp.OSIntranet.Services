using System;
using System.Collections.Generic;
using System.Linq;
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
    /// QueryHandler til håndtering af forespørgelsen: KalenderbrugerAftalerGetQuery.
    /// </summary>
    public class KalenderbrugerAftalerGetQueryHandler : KalenderQueryHandlerBase, IQueryHandler<KalenderbrugerAftalerGetQuery, IEnumerable<KalenderbrugerAftaleView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: KalenderbrugerAftalerGetQuery.
        /// </summary>
        /// <param name="kalenderRepository">Implementering af repository til kalenderdelen under OSWEBDB.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet, såsom systemer under OSWEBDB.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public KalenderbrugerAftalerGetQueryHandler(IKalenderRepository kalenderRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(kalenderRepository, fællesRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<KalenderbrugerAftalerGetQuery,IEnumerable<KalenderbrugerAftaleView>> Members

        /// <summary>
        /// Henter og returnerer kalenderaftaler til en kalenderbruger med et givent sæt initialer.
        /// </summary>
        /// <param name="query">Forespørgelse efter kalenderaftaler til en kalenderbruger med et givent sæt initialer.</param>
        /// <returns>Liste indeholdende kalenderaftaler til kalenderbrugeren med det givne sæt initialer.</returns>
        public IEnumerable<KalenderbrugerAftaleView> Query(KalenderbrugerAftalerGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var system = SystemGetByNummer(query.System);
            var brugere = BrugerlisteGetBySystemAndInitialer(system, query.Initialer);
            var brugeraftaler = KalenderRepository.AftaleGetAllBySystem(system.Nummer, query.FraDato)
                .SelectMany(m => m.Deltagere)
                .ToList();

            return MapMany<IBrugeraftale, KalenderbrugerAftaleView>(brugeraftaler);
        }

        #endregion
    }
}
