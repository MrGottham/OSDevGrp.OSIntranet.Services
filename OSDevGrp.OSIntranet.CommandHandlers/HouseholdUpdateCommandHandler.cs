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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Modifies the data.
        /// </summary>
        /// <param name="household">Household on which to modify data.</param>
        /// <param name="command">Command for updatering a household to the current users household account.</param>
        /// <returns>The updated household.</returns>
        public override IIdentifiable ModifyData(IHousehold household, HouseholdUpdateCommand command)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
