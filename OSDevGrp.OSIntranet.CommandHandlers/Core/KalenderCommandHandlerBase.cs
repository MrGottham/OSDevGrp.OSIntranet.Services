using System;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Basisklasse for en CommandHandler til kalenderdelen under OSWEBDB.
    /// </summary>
    public abstract class KalenderCommandHandlerBase : CommandHandlerTransactionalBase
    {
        #region Private variables

        private readonly IKalenderRepository _kalenderRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for en CommandHandler til kalenderdelen under OSWEBDB.
        /// </summary>
        /// <param name="kalenderRepository">Implementering af repository til kalenderdelen under OSWEBDB.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        protected KalenderCommandHandlerBase(IKalenderRepository kalenderRepository, IObjectMapper objectMapper)
        {
            if (kalenderRepository == null)
            {
                throw new ArgumentNullException("kalenderRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _kalenderRepository = kalenderRepository;
            _objectMapper = objectMapper;
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

        /// <summary>
        /// Objektmapper.
        /// </summary>
        public virtual IObjectMapper ObjectMapper
        {
            get
            {
                return _objectMapper;
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}
