using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// QueryHandler til håndtering af forespørgelsen: BetalingsbetingelserGetQuery.
    /// </summary>
    public class BetalingsbetingelserGetQueryHandler : IQueryHandler<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// QueryHandler til håndtering af forespørgelsen: BetalingsbetingelserGetQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BetalingsbetingelserGetQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _adresseRepository = adresseRepository;
            _objectMapper = objectMapper;
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
            var betalingsbetingelser = _adresseRepository.BetalingsbetingelseGetAll();
            return _objectMapper.Map<IEnumerable<Betalingsbetingelse>, IEnumerable<BetalingsbetingelseView>>(betalingsbetingelser);
        }

        #endregion
    }
}
