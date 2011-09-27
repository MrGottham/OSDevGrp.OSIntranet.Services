using System;
using System.Collections.Generic;
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
    /// QueryHandler til håndtering af forespørgelsen: TelefonlisteGetQuery.
    /// </summary>
    public class TelefonlisteGetQueryHandler : AdressekartotekQueryHandlerBase, IQueryHandler<TelefonlisteGetQuery, IEnumerable<TelefonlisteView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: TelefonlisteGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public TelefonlisteGetQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
            : base(adresseRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<TelefonlisteGetQuery,IEnumerable<TelefonlisteView>> Members

        /// <summary>
        /// Henter og returnerer en telefonliste.
        /// </summary>
        /// <param name="query">Forespørgelse efter en telefonliste.</param>
        /// <returns>Telefonliste.</returns>
        public IEnumerable<TelefonlisteView> Query(TelefonlisteGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var adresser = Repository.AdresseGetAll();

            return MapMany<AdresseBase, TelefonlisteView>(adresser);
        }

        #endregion
    }
}
