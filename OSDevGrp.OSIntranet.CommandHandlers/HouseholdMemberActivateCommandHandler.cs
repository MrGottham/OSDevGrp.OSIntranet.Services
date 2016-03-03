using System;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers
{
    /// <summary>
    /// Command handler which can handles the command for activating the current users household member account.
    /// </summary>
    public class HouseholdMemberActivateCommandHandler : HouseholdMemberDataModificationCommandHandlerBase<HouseholdMemberActivateCommand>
    {
        #region Constructor

        /// <summary>
        /// Creates the command handler which can handles the command for activating the current users household member account.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of a common validations.</param>
        /// <param name="exceptionBuilder">Implementation of a builder which can build exceptions.</param>
        public HouseholdMemberActivateCommandHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder)
            : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
        {
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets whether the household member should be activated to execute the command handled by this command handler.
        /// </summary>
        public override bool ShouldBeActivated
        {
            get { return false; }
        }

        /// <summary>
        /// Gets whether the household member should have accepted the privacy policy to execute the command handled by this command handler.
        /// </summary>
        public override bool ShouldHaveAcceptedPrivacyPolicy
        {
            get { return false; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds validation rules for the command which can activate the current users household member account.
        /// </summary>
        /// <param name="householdMember">Household member which should be activated.</param>
        /// <param name="command">Command for activating the current users household member account.</param>
        /// <param name="specification">Specification which encapsulates validation rules.</param>
        public override void AddValidationRules(IHouseholdMember householdMember, HouseholdMemberActivateCommand command, ISpecification specification)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (specification == null)
            {
                throw new ArgumentNullException("specification");
            }
            specification.IsSatisfiedBy(() => CommonValidations.HasValue(command.ActivationCode), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "ActivationCode")))
                .IsSatisfiedBy(() => CommonValidations.IsLengthValid(command.ActivationCode, 1, 64), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.LengthForPropertyIsInvalid, "ActivationCode", 1, 64)))
                .IsSatisfiedBy(() => CommonValidations.ContainsIllegalChar(command.ActivationCode) == false, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "ActivationCode")))
                .IsSatisfiedBy(() => CommonValidations.Equals(command.ActivationCode, householdMember.ActivationCode), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.WrongActivationCodeForHouseholdMember)));
        }

        /// <summary>
        /// Activate the current users household member account.
        /// </summary>
        /// <param name="householdMember">Household member which should be activated.</param>
        /// <param name="command">Command for activating the current users household member account.</param>
        /// <returns>The activated household member.</returns>
        public override IIdentifiable ModifyData(IHouseholdMember householdMember, HouseholdMemberActivateCommand command)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }

            householdMember.ActivationTime = DateTime.Now;

            return HouseholdDataRepository.Update(householdMember);
        }

        #endregion
    }
}
