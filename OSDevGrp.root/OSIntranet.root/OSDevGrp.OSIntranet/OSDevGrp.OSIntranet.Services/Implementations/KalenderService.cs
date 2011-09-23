using System;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Services;

namespace OSDevGrp.OSIntranet.Services.Implementations
{
    /// <summary>
    /// Service til kalender.
    /// </summary>
    public class KalenderService : IntranetServiceBase, IKalenderService
    {
        #region Private variables

        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner service til kalender.
        /// </summary>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public KalenderService(IQueryBus queryBus)
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