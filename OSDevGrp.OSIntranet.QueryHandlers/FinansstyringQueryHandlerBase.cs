using System;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Basisklasse for en QueryHandler til finansstyring.
    /// </summary>
    public abstract class FinansstyringQueryHandlerBase : QueryHandlerBase
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for en QueryHandler til finansstyring.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        protected FinansstyringQueryHandlerBase(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
            : base(objectMapper)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _finansstyringRepository = finansstyringRepository;
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
