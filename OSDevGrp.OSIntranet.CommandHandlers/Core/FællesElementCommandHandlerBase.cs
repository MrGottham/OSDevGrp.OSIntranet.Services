using System;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Basisklasse for en CommandHandler til fælles elementer i domænet, såsom brevhoveder.
    /// </summary>
    public abstract class FællesElementCommandHandlerBase : CommandHandlerTransactionalBase
    {
        #region Private variables

        private readonly IFællesRepository _fællesRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for en CommandHandler til fælles elementer i domænet, såsom brevhoveder.
        /// </summary>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        protected FællesElementCommandHandlerBase(IFællesRepository fællesRepository, IObjectMapper objectMapper)
        {
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _fællesRepository = fællesRepository;
            _objectMapper = objectMapper;
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
        /// Henter og returnerer et givent brevhoved.
        /// </summary>
        /// <param name="nummer">Unik identifikation af brevhovedet.</param>
        /// <returns>Brevhoved.</returns>
        public virtual Brevhoved BrevhovedGetByNummer(int nummer)
        {
            var brevhovedlisteHelper = new BrevhovedlisteHelper(Repository.BrevhovedGetAll());
            return brevhovedlisteHelper.GetById(nummer);
        }

        #endregion
    }
}
