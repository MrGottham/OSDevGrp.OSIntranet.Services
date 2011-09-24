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
    /// QueryHandler til håndtering af forespørgelsen: PostnumreGetQuery.
    /// </summary>
    public class PostnumreGetQueryHandler : AdressekartotekQueryHandlerBase, IQueryHandler<PostnumreGetQuery, IEnumerable<PostnummerView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: PostnumreGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public PostnumreGetQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
            : base(adresseRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<PostnumreGetQuery,IEnumerable<PostnummerView>> Members

        /// <summary>
        /// Henter og returnerer postnumre.
        /// </summary>
        /// <param name="query">Forespørgelse efter postnumre.</param>
        /// <returns>Liste af postnumre.</returns>
        public IEnumerable<PostnummerView> Query(PostnumreGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var postnumre = Repository.PostnummerGetAll();

            return MapMany<Postnummer, PostnummerView>(postnumre);
        }

        #endregion
    }
}
