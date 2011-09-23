using System;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Services;

namespace OSDevGrp.OSIntranet.Services.Implementations
{
    /// <summary>
    /// Service til adressekartotek.
    /// </summary>
    public class AdressekartotekService : IntranetServiceBase, IAdressekartotekService
    {
        #region Private variables

        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner service til adressekartotek.
        /// </summary>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public AdressekartotekService(IQueryBus queryBus)
        {
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            _queryBus = queryBus;
        }

        #endregion
    }
}