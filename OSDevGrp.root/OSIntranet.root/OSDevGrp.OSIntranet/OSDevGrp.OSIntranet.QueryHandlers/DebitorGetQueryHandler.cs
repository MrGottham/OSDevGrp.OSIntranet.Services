using System;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// /// QueryHandler til håndtering af forespørgelsen: DebitorGetQuery.
    /// </summary>
    public class DebitorGetQueryHandler : AdressekontoQueryHandlerBase, IQueryHandler<DebitorGetQuery, DebitorView>
    {
        #region Private variables

        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: DebitorGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public DebitorGetQueryHandler(IAdresseRepository adresseRepository, IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
            : base(adresseRepository, finansstyringRepository)
        {
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _objectMapper = objectMapper;
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
            var adressekonto = AdressekontoGetByRegnskabsnummerAndNummer(query.Regnskabsnummer, query.StatusDato,
                                                                         query.Nummer);
            return _objectMapper.Map<AdresseBase, DebitorView>(adressekonto);
        }

        #endregion
    }
}
