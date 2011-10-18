using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
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
        private readonly IFællesRepository _fællesRepository;

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

        #region Methods

        /// <summary>
        /// Henter alle kalenderbrugere med et givent sæt initialer i et system under OSWEBDB.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet under OSWEBDB.</param>
        /// <param name="initialer">Initialer for de brugere, som skal hentes.</param>
        /// <returns>Brugere med det givne sæt initialer.</returns>
        public virtual IEnumerable<IBruger> BrugerGetBySystemAndInitialer(int system, string initialer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
