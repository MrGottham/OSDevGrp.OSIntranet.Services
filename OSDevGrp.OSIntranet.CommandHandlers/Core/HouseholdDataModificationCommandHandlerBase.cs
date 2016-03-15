using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Basic functionality which can handle a command for modifying some data on a given household on the current household member.
    /// </summary>
    /// <typeparam name="TCommand">Type of the command for modifying some data on a given household on the current household member.</typeparam>
    public abstract class HouseholdDataModificationCommandHandlerBase<TCommand> : HouseholdMemberDataModificationCommandHandlerBase<TCommand> where TCommand : HouseholdDataModificationCommandBase
    {
        #region Constructor

        /// <summary>
        /// Creates the basic functionality which can handle a command for modifying some data on a given household on the current household member.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of a common validations.</param>
        /// <param name="exceptionBuilder">Implementation of a builder which can build exceptions.</param>
        protected HouseholdDataModificationCommandHandlerBase(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder)
            : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds validation rules to the specification which encapsulates validation rules.
        /// </summary>
        /// <param name="householdMember">Household member for which to modify data.</param>
        /// <param name="command">Command for modifying some data on a given household on the current household member.</param>
        /// <param name="specification">Specification which encapsulates validation rules.</param>
        public override void AddValidationRules(IHouseholdMember householdMember, TCommand command, ISpecification specification)
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

            var household = householdMember.Households.SingleOrDefault(m => m.Identifier.HasValue && m.Identifier.Value == command.HouseholdIdentifier);

            specification.IsSatisfiedBy(() => CommonValidations.IsNotNull(household), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, command.HouseholdIdentifier)))
                .Evaluate();
        }

        /// <summary>
        /// Modifies the data.
        /// </summary>
        /// <param name="householdMember">Household member for which to modify data.</param>
        /// <param name="command">Command for modifying some data on a given household on the current household member.</param>
        /// <returns>An identifiable domain object in the food waste domain.</returns>
        public override IIdentifiable ModifyData(IHouseholdMember householdMember, TCommand command)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var household = householdMember.Households.SingleOrDefault(m => m.Identifier.HasValue && m.Identifier.Value == command.HouseholdIdentifier);

            Specification.IsSatisfiedBy(() => CommonValidations.IsNotNull(household), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, command.HouseholdIdentifier)))
                .Evaluate();

            return null;
        }

        /// <summary>
        /// Adds validation rules to the specification which encapsulates validation rules.
        /// </summary>
        /// <param name="household">Household on which to modify data.</param>
        /// <param name="command">Command for modifying some data on a given household on the current household member.</param>
        /// <param name="specification">Specification which encapsulates validation rules.</param>
        public abstract void AddValidationRules(IHousehold household, TCommand command, ISpecification specification);

        #endregion
    }
}
