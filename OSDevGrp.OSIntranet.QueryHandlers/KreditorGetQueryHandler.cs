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
    public class KreditorGetQueryHandler : AdressekontoQueryHandlerBase, IQueryHandler<KreditorGetQuery, KreditorView>
    {
        #region Private variables

        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: KreditorGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public KreditorGetQueryHandler(IAdresseRepository adresseRepository, IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
            : base(adresseRepository, finansstyringRepository)
        {
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _objectMapper = objectMapper;
        }

        #endregion

        #region IQueryHandler<KreditorGetQuery,KreditorView> Members

        /// <summary>
        /// Henter og returnerer en kreditor.
        /// </summary>
        /// <param name="query">Forespørgelse til at hente en kreditor.</param>
        /// <returns>Kreditor.</returns>
        public KreditorView Query(KreditorGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var adressekonto = AdressekontoGetByRegnskabsnummerAndNummer(query.Regnskabsnummer, query.StatusDato,
                                                                         query.Nummer);
            return _objectMapper.Map<AdresseBase, KreditorView>(adressekonto);
        }

        #endregion
    }
}
