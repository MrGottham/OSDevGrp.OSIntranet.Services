using System;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers.Core
{
    /// <summary>
    /// Basisklasse for en QueryHandler til kalenderdelen under OSWEBDB.
    /// </summary>
    public abstract class KalenderQueryHandlerBase : QueryHandlerBase
    {
        #region Private variables

        private readonly IKalenderRepository _kalenderRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for en QueryHandler til kalenderdelen under OSWEBDB.
        /// </summary>
        /// <param name="kalenderRepository">Implementering af repository til kalenderdelen under OSWEBDB.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        protected KalenderQueryHandlerBase(IKalenderRepository kalenderRepository, IObjectMapper objectMapper)
            : base(objectMapper)
        {
            if (kalenderRepository == null)
            {
                throw new ArgumentNullException("kalenderRepository");
            }
            _kalenderRepository = kalenderRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Repository til kalenderdelen under OSWEBDB.
        /// </summary>
        public virtual IKalenderRepository Repository
        {
            get
            {
                return _kalenderRepository;
            }
        }

        #endregion
    }
}
