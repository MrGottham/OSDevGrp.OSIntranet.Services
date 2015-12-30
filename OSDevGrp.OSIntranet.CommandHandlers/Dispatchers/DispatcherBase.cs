using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers.Dispatchers
{
    /// <summary>
    /// Basic functionaly for a dispatcher which can dispatch data for a given domain object in the food waste domain.
    /// </summary>
    /// <typeparam name="TDomainObject">Type of the domain object which provides data to dispatch.</typeparam>
    public abstract class DispatcherBase<TDomainObject> : IDispatcher<TDomainObject> where TDomainObject : IIdentifiable
    {
        #region Private variables

        private readonly ICommunicationRepository _communicationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the basic functionaly for a dispatcher which can dispatch data for a given domain object in the food waste domain.
        /// </summary>
        /// <param name="communicationRepository">Implementation of a repository used for communication with internal and external stakeholders in the food waste domain.</param>
        protected DispatcherBase(ICommunicationRepository communicationRepository)
        {
            if (communicationRepository == null)
            {
                throw new ArgumentNullException("communicationRepository");
            }
            _communicationRepository = communicationRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Repository used for communication with internal and external stakeholders in the food waste domain.
        /// </summary>
        protected virtual ICommunicationRepository CommunicationRepository
        {
            get { return _communicationRepository; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dispatch data for a given domain object in the food waste domain.
        /// </summary>
        /// <param name="stakeholder">Stakeholder which data should be dispatched to.</param>
        /// <param name="domainObject">Domain object which provides data to dispatch.</param>
        /// <param name="translationInfo">Translation informations used to translate the data to dispatch.</param>
        public virtual void Dispatch(IStakeholder stakeholder, TDomainObject domainObject, ITranslationInfo translationInfo)
        {
            if (stakeholder == null)
            {
                throw new ArgumentNullException("stakeholder");
            }
            if (Equals(domainObject, null))
            {
                throw new ArgumentNullException("domainObject");
            }
            if (translationInfo == null)
            {
                throw new ArgumentNullException("translationInfo");
            }

            var translatableDomainObject = domainObject as ITranslatable;
            if (translatableDomainObject != null)
            {
                translatableDomainObject.Translate(translationInfo.CultureInfo);
            }

            HandleCommunication(stakeholder, domainObject, translationInfo);
        }

        /// <summary>
        /// Handles the communication so data will be dispatched to the stakeholder.
        /// </summary>
        /// <param name="stakeholder">Stakeholder which data should be dispatched to.</param>
        /// <param name="domainObject">Domain object which provides data to dispatch.</param>
        /// <param name="translationInfo">Translation informations used to translate the data to dispatch.</param>
        protected abstract void HandleCommunication(IStakeholder stakeholder, TDomainObject domainObject, ITranslationInfo translationInfo);

        #endregion
    }
}
