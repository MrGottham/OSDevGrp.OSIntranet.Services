using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: RegnskabslisteGetQuery.
    /// </summary>
    public class RegnskabslisteGetQueryHandler : FinansstyringQueryHandlerBase, IQueryHandler<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>
    {
        #region Private variables

        private readonly IFællesRepository _fællesRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: RegnskabslisteGetQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public RegnskabslisteGetQueryHandler(IFinansstyringRepository finansstyringRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, objectMapper)
        {
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            _fællesRepository = fællesRepository;
        }

        #endregion

        #region IQueryHandler<RegnskabslisteGetQuery,IEnumerable<RegnskabslisteView>> Members

        /// <summary>
        /// Henter og returnerer en regnskabsliste.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en regnskabsliste.</param>
        /// <returns>Regnskabsliste.</returns>
        public IEnumerable<RegnskabslisteView> Query(RegnskabslisteGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var brevhovedlisteHelper = new BrevhovedlisteHelper(_fællesRepository.BrevhovedGetAll());

            var regnskaber = Repository.RegnskabslisteGet(brevhovedlisteHelper.GetById);

            return MapMany<Regnskab, RegnskabslisteView>(regnskaber);
        }

        #endregion
    }
}
