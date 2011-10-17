using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: SystemerGetQuery.
    /// </summary>
    public class SystemerGetQueryHandler : FællesElementQueryHandlerBase, IQueryHandler<SystemerGetQuery, IEnumerable<SystemView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: SystemerGetQuery.
        /// </summary>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public SystemerGetQueryHandler(IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(fællesRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<SystemerGetQuery,IEnumerable<SystemView>> Members

        /// <summary>
        /// Henter og returnerer systemer under OSWEBDB.
        /// </summary>
        /// <param name="query">Foresprøgelser efter systemer under OSWEBDB.</param>
        /// <returns>Liste indeholdende systemer under OSWEBDB.</returns>
        public IEnumerable<SystemView> Query(SystemerGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var systemer = Repository.SystemGetAll();

            return MapMany<ISystem, SystemView>(systemer);
        }

        #endregion
    }
}
