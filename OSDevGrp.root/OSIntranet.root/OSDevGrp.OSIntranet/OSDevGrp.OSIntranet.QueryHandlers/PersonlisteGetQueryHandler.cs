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
    /// QueryHandler til håndtering af forespørgelsen: PersonlisteGetQuery.
    /// </summary>
    public class PersonlisteGetQueryHandler : AdressekartotekQueryHandlerBase, IQueryHandler<PersonlisteGetQuery, IEnumerable<PersonView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: PersonlisteGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public PersonlisteGetQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
            : base(adresseRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<PersonlisteGetQuery,IEnumerable<PersonView>> Members

        /// <summary>
        /// Henter og returnerer en liste af personer.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af personer.</param>
        /// <returns>Liste af personer.</returns>
        public IEnumerable<PersonView> Query(PersonlisteGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var personer = PersonGetAll();

            return MapMany<Person, PersonView>(personer);
        }

        #endregion
    }
}
