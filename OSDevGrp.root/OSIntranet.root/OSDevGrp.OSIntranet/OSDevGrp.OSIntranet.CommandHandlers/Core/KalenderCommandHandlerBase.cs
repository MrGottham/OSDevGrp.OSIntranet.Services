using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.Comparers;
using OSDevGrp.OSIntranet.Domain.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

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

        /// <summary>
        /// Henter og returnerer et givent system under OSWEBDB.
        /// </summary>
        /// <param name="nummer">Unik identifikation af systemet.</param>
        /// <returns>System under OSWEBDB.</returns>
        public virtual ISystem SystemGetByNummer(int nummer)
        {
            var systemlisteHelper = new SystemlisteHelper(FællesRepository.SystemGetAll());
            return systemlisteHelper.GetById(nummer);
        }

        /// <summary>
        /// Henter alle kalenderbrugere med et givent sæt initialer i et system under OSWEBDB.
        /// </summary>
        /// <param name="system">System, hvorfra kalenderbrugere med initialerne skal hentes..</param>
        /// <param name="initialer">Initialer for de brugere, som skal hentes.</param>
        /// <returns>Brugere med det givne sæt initialer.</returns>
        public virtual IEnumerable<IBruger> BrugerlisteGetBySystemAndInitialer(ISystem system, string initialer)
        {
            if (system == null)
            {
                throw new ArgumentNullException("system");
            }
            if (string.IsNullOrEmpty(initialer))
            {
                throw new ArgumentNullException("initialer");
            }

            var comparer = new KalenderbrugerComparer();
            var brugerliste = KalenderRepository.BrugerGetAllBySystem(system.Nummer)
                .Where(m => m.Initialer != null && m.Initialer.CompareTo(initialer) == 0)
                .OrderBy(m => m, comparer)
                .ToList();
            if (brugerliste.Count == 0)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.NoCalendarUserWithThoseInitials, initialer));
            }

            return brugerliste;
        }

        #endregion
    }
}
