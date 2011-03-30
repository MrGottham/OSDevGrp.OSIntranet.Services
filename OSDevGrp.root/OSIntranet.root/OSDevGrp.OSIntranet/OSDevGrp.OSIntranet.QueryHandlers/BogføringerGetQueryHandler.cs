using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Comparers;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: BogføringerGetQuery.
    /// </summary>
    public class BogføringerGetQueryHandler : IQueryHandler<BogføringerGetQuery, IEnumerable<BogføringslinjeView>>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: BogføringerGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BogføringerGetQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IObjectMapper objectMapper)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _finansstyringRepository = finansstyringRepository;
            _adresseRepository = adresseRepository;
            _objectMapper = objectMapper;
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
            var adresser = _adresseRepository.AdresseGetAll();
            var regnskab = _finansstyringRepository.RegnskabGet(query.Regnskabsnummer, nummer =>
                                                                                           {
                                                                                               var adresse = adresser.SingleOrDefault(m => m.Nummer == nummer);
                                                                                               if (adresse == null)
                                                                                               {
                                                                                                   var message = Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, "AdresseBase", nummer);
                                                                                                   throw new IntranetRepositoryException(message);
                                                                                               }
                                                                                               return adresse;
                                                                                           });

            var bogføringslinjeComparer = new BogføringslinjeComparer();
            var bogføringslinjer = regnskab.Konti.OfType<Konto>()
                .SelectMany(m => m.Bogføringslinjer)
                .OrderByDescending(m => m, bogføringslinjeComparer)
                .Where(m => m.Dato.CompareTo(query.StatusDato) <= 0)
                .Take(query.Linjer)
                .ToList();

            return bogføringslinjer.Select(bogføringslinje => _objectMapper.Map<Bogføringslinje, BogføringslinjeView>(bogføringslinje)).ToList();
        }

        #endregion
    }
}
