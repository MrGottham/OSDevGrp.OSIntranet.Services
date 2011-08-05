using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: BrevhovederGetQuery.
    /// </summary>
    public class BrevhovederGetQueryHandler : FællesElementQueryHandlerBase, IQueryHandler<BrevhovederGetQuery, IEnumerable<BrevhovedView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: BrevhovederGetQuery.
        /// </summary>
        /// <param name="fællesRepository">Implementering af objectmapper.</param>
        /// <param name="objectMapper">Implementering af repository til fælles elementer i domænet.</param>
        public BrevhovederGetQueryHandler(IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(fællesRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<BrevhovederGetQuery,IEnumerable<BrevhovedView>> Members

        /// <summary>
        /// Henter og returnerer brevhoveder.
        /// </summary>
        /// <param name="query">Forespørgelse efter brevhoveder.</param>
        /// <returns>Liste af brevhoveder.</returns>
        public IEnumerable<BrevhovedView> Query(BrevhovederGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var brevhoveder = Repository.BrevhovedGetAll();

            return MapMany<Brevhoved, BrevhovedView>(brevhoveder);
        }

        #endregion
    }
}
