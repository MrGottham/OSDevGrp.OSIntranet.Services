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
        private readonly IExceptionBuilder _exceptionBuilder;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for en CommandHandler til fælles elementer i domænet, såsom brevhoveder.
        /// </summary>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        /// <param name="exceptionBuilder">Implementering af en builder, der kan bygge exceptions.</param>
        protected FællesElementCommandHandlerBase(IFællesRepository fællesRepository, IObjectMapper objectMapper, IExceptionBuilder exceptionBuilder)
        {
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            if (exceptionBuilder == null)
            {
                throw new ArgumentNullException("exceptionBuilder");
            }
            _fællesRepository = fællesRepository;
            _objectMapper = objectMapper;
            _exceptionBuilder = exceptionBuilder;
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

        /// <summary>
        /// Builder, der kan bygge exceptions.
        /// </summary>
        public virtual IExceptionBuilder ExceptionBuilder
        {
            get
            {
                return _exceptionBuilder;
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
