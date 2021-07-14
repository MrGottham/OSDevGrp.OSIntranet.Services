using System;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers.Core
{
    /// <summary>
    /// Basisklasse for en QueryHandler til fælles elementer i domænet, såsom brevhoved.
    /// </summary>
    public abstract class FællesElementQueryHandlerBase : QueryHandlerBase
    {
        #region Private variables

        private readonly IFællesRepository _fællesRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for en QueryHandler til fælles elementer i domænet, såsom brevhoved.
        /// </summary>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        protected FællesElementQueryHandlerBase(IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(objectMapper)
        {
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            _fællesRepository = fællesRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Repository til fælles elementer i domænet.
        /// </summary>
        public virtual IFællesRepository Repository
        {
            get
            {
                return _fællesRepository;
            }
        }

        #endregion
    }
}
