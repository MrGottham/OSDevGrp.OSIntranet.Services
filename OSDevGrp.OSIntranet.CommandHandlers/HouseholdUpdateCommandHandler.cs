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
    /// Command handler which handles a command for updatering a household to the current users household account.
    /// </summary>
    public class HouseholdUpdateCommandHandler : HouseholdDataModificationCommandHandlerBase<HouseholdUpdateCommand>
    {
        #region Constructor

        /// <summary>
        /// Creates a command handler which handles a command for updatering a household to the current users household account.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of a common validations.</param>
        /// <param name="exceptionBuilder">Implementation of a builder which can build exceptions.</param>
        public HouseholdUpdateCommandHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder)
            : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds validation rules to the specification which encapsulates validation rules.
        /// </summary>
        /// <param name="household">Household on which to modify data.</param>
        /// <param name="command">Command for updatering a household to the current users household account.</param>
        /// <param name="specification">Specification which encapsulates validation rules.</param>
        public override void AddValidationRules(IHousehold household, HouseholdUpdateCommand command, ISpecification specification)
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

            specification.IsSatisfiedBy(() => CommonValidations.HasValue(command.Name), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Name")))
                .IsSatisfiedBy(() => CommonValidations.IsLengthValid(command.Name, 1, 64), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.LengthForPropertyIsInvalid, "Name", 1, 64)))
                .IsSatisfiedBy(() => CommonValidations.ContainsIllegalChar(command.Name) == false, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "Name")))
                .IsSatisfiedBy(() => command.Description == null || CommonValidations.HasValue(command.Description), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Description")))
                .IsSatisfiedBy(() => command.Description == null || CommonValidations.IsLengthValid(command.Description, 1, 2048), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.LengthForPropertyIsInvalid, "Description", 1, 2048)))
                .IsSatisfiedBy(() => command.Description == null || CommonValidations.ContainsIllegalChar(command.Description) == false, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "Description")));
        }

        /// <summary>
        /// Modifies the data.
        /// </summary>
        /// <param name="household">Household on which to modify data.</param>
        /// <param name="command">Command for updatering a household to the current users household account.</param>
        /// <returns>The updated household.</returns>
        public override IIdentifiable ModifyData(IHousehold household, HouseholdUpdateCommand command)
        {
            if (household == null)
            {
                throw new ArgumentNullException("household");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            household.Name = command.Name;
            household.Description = command.Description;

            return HouseholdDataRepository.Update(household);
        }

        #endregion
    }
}
