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
    /// QueryHandler til håndtering af forespørgelsen: FirmalisteGetQuery.
    /// </summary>
    public class FirmalisteGetQueryHandler : AdressekartotekQueryHandlerBase, IQueryHandler<FirmalisteGetQuery, IEnumerable<FirmaView>>
    {
        #region Constructor

        /// <summary>
        /// Danner QueryHandler til håndtering af forespørgelsen: FirmalisteGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public FirmalisteGetQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
            : base(adresseRepository, objectMapper)
        {
        }

        #endregion

        #region IQueryHandler<FirmalisteGetQuery,IEnumerable<FirmaView>> Members

        /// <summary>
        /// Henter og returnerer en liste af firmaer.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af firmaer.</param>
        /// <returns>Liste af firmaer.</returns>
        public IEnumerable<FirmaView> Query(FirmalisteGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var firmaer = FirmaGetAll();

            return MapMany<Firma, FirmaView>(firmaer);
        }

        #endregion
    }
}
