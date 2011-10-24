using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: KalenderbrugerAftaleGetQuery.
    /// </summary>
    public class KalenderbrugerAftaleGetQueryHandler : KalenderQueryHandlerBase, IQueryHandler<KalenderbrugerAftaleGetQuery, KalenderbrugerAftaleView>
    {
        #region Constructor
        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: KalenderbrugerAftaleGetQuery.
        /// </summary>
        /// <param name="kalenderRepository">Implementering af repository til kalenderdelen under OSWEBDB.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet, såsom systemer under OSWEBDB.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public KalenderbrugerAftaleGetQueryHandler(IKalenderRepository kalenderRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(kalenderRepository, fællesRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<KalenderbrugerAftaleGetQuery,KalenderbrugerAftaleView> Members

        /// <summary>
        /// Henter og returnerer en given kalenderaftale til en kalenderbruger med et givent sæt initialer.
        /// </summary>
        /// <param name="query">Forespørgelse efter en given kalenderaftale til en kalenderbruger med et givent sæt initialer.</param>
        /// <returns>Kalenderaftale til en kalenderbruger med et givent sæt initialer.</returns>
        public KalenderbrugerAftaleView Query(KalenderbrugerAftaleGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var system = SystemGetByNummer(query.System);
            var brugere = BrugerlisteGetBySystemAndInitialer(system, query.Initialer);
            var brugeraftale = KalenderRepository.AftaleGetBySystemAndId(system.Nummer, query.AftaleId).Deltagere
                .Where(m => brugere.SingleOrDefault(n => n.Id == m.Bruger.Id) != null)
                .FirstOrDefault();
            if (brugeraftale == null)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.UserAppointmentDontExists));
            }

            return Map<IBrugeraftale, KalenderbrugerAftaleView>(brugeraftale);
        }

        #endregion
    }
}
