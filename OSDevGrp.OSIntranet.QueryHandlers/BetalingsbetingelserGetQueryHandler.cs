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
    /// QueryHandler til håndtering af forespørgelsen: BetalingsbetingelserGetQuery.
    /// </summary>
    public class BetalingsbetingelserGetQueryHandler : AdressekartotekQueryHandlerBase, IQueryHandler<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: BetalingsbetingelserGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BetalingsbetingelserGetQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
            : base(adresseRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<BetalingsbetingelserGetQuery,IEnumerable<BetalingsbetingelseView>> Members

        /// <summary>
        /// Henter og returnerer betalingsbetingelser.
        /// </summary>
        /// <param name="query">Forespørgelse efter betalingsbetingelser.</param>
        /// <returns>Liste af betalingsbetingelser.</returns>
        public IEnumerable<BetalingsbetingelseView> Query(BetalingsbetingelserGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
        
            var betalingsbetingelser = Repository.BetalingsbetingelseGetAll();
            
            return MapMany<Betalingsbetingelse, BetalingsbetingelseView>(betalingsbetingelser);
        }

        #endregion
    }
}
