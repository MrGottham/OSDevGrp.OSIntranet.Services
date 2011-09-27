using System;
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
    /// QueryHandler til håndtering af forespørgelsen: PersonGetQuery.
    /// </summary>
    public class PersonGetQueryHandler : AdressekartotekQueryHandlerBase, IQueryHandler<PersonGetQuery, PersonView>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: PersonGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public PersonGetQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
            : base(adresseRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<PersonGetQuery,PersonView> Members

        /// <summary>
        /// Henter og returnerer en given person.
        /// </summary>
        /// <param name="query">Forespørgelse efter en given person.</param>
        /// <returns>Person.</returns>
        public PersonView Query(PersonGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var person = PersonGetByNummer(query.Nummer);

            return Map<Person, PersonView>(person);
        }

        #endregion
    }
}
