using System;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Basisklasse for en CommandHandler til finansstyring.
    /// </summary>
    public abstract class FinansstyringCommandHandlerBase : CommandHandlerTransactionalBase
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for en CommandHandler til finansstyring.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        protected FinansstyringCommandHandlerBase(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _finansstyringRepository = finansstyringRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Repository til finansstyring.
        /// </summary>
        public virtual IFinansstyringRepository Repository
        {
            get
            {
                return _finansstyringRepository;
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
        /// Henter og returnerer en given kontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <returns>Kontogruppe.</returns>
        public virtual Kontogruppe KontogruppeGetByNummer(int nummer)
        {
            var kontogruppelisteHelper = new KontogruppelisteHelper(Repository.KontogruppeGetAll());
            return kontogruppelisteHelper.GetById(nummer);
        }

        /// <summary>
        /// Henter og returnerer en given gruppe til budgetkonti.
        /// </summary>
        /// <param name="nummer">Unik identifikation af gruppen til budgetkonti.</param>
        /// <returns>Gruppe til budgetkonti.</returns>
        public virtual Budgetkontogruppe BudgetkontogruppeGetByNummer(int nummer)
        {
            var budgetkontogruppelisteHelper = new BudgetkontogruppelisteHelper(Repository.BudgetkontogruppeGetAll());
            return budgetkontogruppelisteHelper.GetById(nummer);
        }

        #endregion
    }
}
