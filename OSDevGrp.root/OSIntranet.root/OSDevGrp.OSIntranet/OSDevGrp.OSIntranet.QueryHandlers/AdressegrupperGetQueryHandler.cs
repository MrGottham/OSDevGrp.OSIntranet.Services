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
    /// QueryHandler til håndtering af forespørgelsen: AdressegrupperGetQuery.
    /// </summary>
    public class AdressegrupperGetQueryHandler : AdressekartotekQueryHandlerBase, IQueryHandler<AdressegrupperGetQuery, IEnumerable<AdressegruppeView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: AdressegrupperGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public AdressegrupperGetQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
            : base(adresseRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<AdressegrupperGetQuery,IEnumerable<AdressegruppeView>> Members

        /// <summary>
        /// Henter og returnerer adressegrupper.
        /// </summary>
        /// <param name="query">Forespørgelse efter adressegrupper.</param>
        /// <returns>Liste af adressegrupper.</returns>
        public IEnumerable<AdressegruppeView> Query(AdressegrupperGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var adressegrupper = Repository.AdressegruppeGetAll();

            return MapMany<Adressegruppe, AdressegruppeView>(adressegrupper);
        }

        #endregion
    }
}
