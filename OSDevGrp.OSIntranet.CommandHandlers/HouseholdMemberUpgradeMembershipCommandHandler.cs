using System;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers
{
    /// <summary>
    /// Command handler which can handles the command for upgrading the membership on the current users household member account.
    /// </summary>
    public class HouseholdMemberUpgradeMembershipCommandHandler : HouseholdMemberDataModificationCommandHandlerBase<HouseholdMemberUpgradeMembershipCommand>
    {
        #region Constructor

        /// <summary>
        /// Creates the command handler which can handles the command for upgrading the membership on the current users household member account.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of a common validations.</param>
        /// <param name="exceptionBuilder">Implementation of a builder which can build exceptions.</param>
        public HouseholdMemberUpgradeMembershipCommandHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder)
            : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds validation rules for the command for upgrading the membership on the current users household member account.
        /// </summary>
        /// <param name="householdMember">Household member on which the membership should be upgraded.</param>
        /// <param name="command">Command for upgrading the membership on the current users household member account.</param>
        /// <param name="specification">Specification which encapsulates validation rules.</param>
        public override void AddValidationRules(IHouseholdMember householdMember, HouseholdMemberUpgradeMembershipCommand command, ISpecification specification)
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
        }

        /// <summary>
        /// Upgrade membership on the current users household member account.
        /// </summary>
        /// <param name="householdMember">Household member on which the membership should be upgraded.</param>
        /// <param name="command">Command for upgrading the membership on the current users household member account.</param>
        /// <returns>Household member with the upgraded membership.</returns>
        public override IIdentifiable ModifyData(IHouseholdMember householdMember, HouseholdMemberUpgradeMembershipCommand command)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            return null;
        }

        #endregion
    }
}
