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
        private readonly IFællesRepository _fællesRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for en CommandHandler til kalenderdelen under OSWEBDB.
        /// </summary>
        /// <param name="kalenderRepository">Implementering af repository til kalenderdelen under OSWEBDB.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet, såsom systemer under OSWEBDB.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        protected KalenderCommandHandlerBase(IKalenderRepository kalenderRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
        {
            if (kalenderRepository == null)
            {
                throw new ArgumentNullException("kalenderRepository");
            }
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _kalenderRepository = kalenderRepository;
            _fællesRepository = fællesRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Repository til kalenderdelen under OSWEBDB.
        /// </summary>
        public virtual IKalenderRepository KalenderRepository
        {
            get
            {
                return _kalenderRepository;
            }
        }

        /// <summary>
        /// Repository til fælles elementer i domænet, såsom systemer under OSWEBDB.
        /// </summary>
        public virtual IFællesRepository FællesRepository
        {
            get
            {
                return _fællesRepository;
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
