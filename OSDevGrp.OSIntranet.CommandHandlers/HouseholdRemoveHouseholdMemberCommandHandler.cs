using System;
using System.Linq;
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
    /// Command handler which handles a command for removing a household member from a given household on the current users household account.
    /// </summary>
    public class HouseholdRemoveHouseholdMemberCommandHandler : HouseholdDataModificationCommandHandlerBase<HouseholdRemoveHouseholdMemberCommand>
    {
        #region Private variables

        private readonly IDomainObjectValidations _domainObjectValidations;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a command handler which handles a command for removing a household member from a given household on the current users household account.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of common validations.</param>
        /// <param name="domainObjectValidations">Implemenation of common validations used by domain objects in the food waste domain.</param>
        /// <param name="exceptionBuilder">Implementation of a builder which can build exceptions.</param>
        public HouseholdRemoveHouseholdMemberCommandHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IDomainObjectValidations domainObjectValidations, IExceptionBuilder exceptionBuilder)
            : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
        {
            if (domainObjectValidations == null)
            {
                throw new ArgumentNullException("domainObjectValidations");
            }
            _domainObjectValidations = domainObjectValidations;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds validation rules to the specification which encapsulates validation rules.
        /// </summary>
        /// <param name="household">Household on which to modify data.</param>
        /// <param name="command">Command for removing a household member from a given household on the current users household account.</param>
        /// <param name="specification">Specification which encapsulates validation rules.</param>
        public override void AddValidationRules(IHousehold household, HouseholdRemoveHouseholdMemberCommand command, ISpecification specification)
        {
            if (household == null)
            {
                throw new ArgumentNullException("household");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (specification == null)
            {
                throw new ArgumentNullException("specification");
            }

            var currentMailAddress = ClaimValueProvider.MailAddress;
            var mailAddressToRemove = command.MailAddress;

            specification.IsSatisfiedBy(() => CommonValidations.HasValue(command.MailAddress), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "MailAddress")))
                .IsSatisfiedBy(() => CommonValidations.IsLengthValid(command.MailAddress, 1, 128), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.LengthForPropertyIsInvalid, "MailAddress", 1, 128)))
                .IsSatisfiedBy(() => CommonValidations.ContainsIllegalChar(command.MailAddress) == false, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "MailAddress")))
                .IsSatisfiedBy(() => _domainObjectValidations.IsMailAddress(command.MailAddress), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, command.MailAddress, "MailAddress")))
                .IsSatisfiedBy(() => CommonValidations.Equals(mailAddressToRemove, currentMailAddress, StringComparison.OrdinalIgnoreCase) == false, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.CannotModifyHouseholdMembershipForYourself)));
        }

        /// <summary>
        /// Modifies the data.
        /// </summary>
        /// <param name="household">Household on which to modify data.</param>
        /// <param name="command">Command for removing a household member from a given household on the current users household account.</param>
        /// <returns>The updated household.</returns>
        public override IIdentifiable ModifyData(IHousehold household, HouseholdRemoveHouseholdMemberCommand command)
        {
            if (household == null)
            {
                throw new ArgumentNullException("household");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var householdMemberForMailAddress = household.HouseholdMembers.SingleOrDefault(householdMember => string.Compare(householdMember.MailAddress, command.MailAddress, StringComparison.OrdinalIgnoreCase) == 0);

            return null;
        }

        #endregion
    }
}
