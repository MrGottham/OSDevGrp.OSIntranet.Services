using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers.Dispatchers
{
    /// <summary>
    /// Dispatcher which can dispatch the welcome letter to a household member.
    /// </summary>
    public class WelcomeLetterDispatcher : DispatcherBase<IHouseholdMember>, IWelcomeLetterDispatcher
    {
        #region Private variables

        private readonly ISystemDataRepository _systemDataRepository;
        private readonly IStaticTextFieldMerge _staticTextFieldMerge;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a dispatcher which can dispatch the welcome letter to a household member.
        /// </summary>
        /// <param name="communicationRepository">Implementation of a repository used for communication with internal and external stakeholders in the food waste domain.</param>
        /// <param name="systemDataRepository">Implementation of a repository which can access system data for the food waste domain.</param>
        /// <param name="staticTextFieldMerge">Implementation of the functionality which can merge fields in a static text.</param>
        public WelcomeLetterDispatcher(ICommunicationRepository communicationRepository, ISystemDataRepository systemDataRepository, IStaticTextFieldMerge staticTextFieldMerge)
            : base(communicationRepository)
        {
            if (systemDataRepository == null)
            {
                throw new ArgumentNullException("systemDataRepository");
            }
            if (staticTextFieldMerge == null)
            {
                throw new ArgumentNullException("staticTextFieldMerge");
            }
            _systemDataRepository = systemDataRepository;
            _staticTextFieldMerge = staticTextFieldMerge;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the communication so welcome letter will be dispatched to the household member.
        /// </summary>
        /// <param name="stakeholder">Stakeholder which data should be dispatched to.</param>
        /// <param name="householdMember">Household member which should receive the welcome letter.</param>
        /// <param name="translationInfo">Translation informations used to translate the data to dispatch.</param>
        protected override void HandleCommunication(IStakeholder stakeholder, IHouseholdMember householdMember, ITranslationInfo translationInfo)
        {
            var welcomeLetterStaticText = _systemDataRepository.StaticTextGetByStaticTextType(StaticTextType.WelcomeLetter);
            welcomeLetterStaticText.Translate(translationInfo.CultureInfo);

            _staticTextFieldMerge.AddMergeFields(householdMember, translationInfo);
            _staticTextFieldMerge.Merge(welcomeLetterStaticText, translationInfo);

           CommunicationRepository.SendMail(stakeholder.MailAddress, welcomeLetterStaticText.SubjectTranslation.Value, welcomeLetterStaticText.BodyTranslation.Value);
        }

        #endregion
    }
}
