using System;
using System.Reflection;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers
{
    /// <summary>
    /// Command handler which handles a command for adding a household to the current users household account.
    /// </summary>
    public class HouseholdAddCommandHandler : FoodWasteHouseholdDataCommandHandlerBase, ICommandHandler<HouseholdAddCommand, ServiceReceiptResponse>
    {
        #region Private variables

        private readonly IClaimValueProvider _claimValueProvider;
        private readonly ILogicExecutor _logicExecutor;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a command handler which handles a command for adding a household to the current users household account.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of a common validations.</param>
        /// <param name="logicExecutor">Implementation of the logic executor which can execute basic logic.</param>
        /// <param name="exceptionBuilder">Implementation of a builder which can build exceptions.</param>
        public HouseholdAddCommandHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, ILogicExecutor logicExecutor, IExceptionBuilder exceptionBuilder)
            : base(householdDataRepository, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
        {
            if (claimValueProvider == null)
            {
                throw new ArgumentNullException("claimValueProvider");
            }
            if (logicExecutor == null)
            {
                throw new ArgumentNullException("logicExecutor");
            }
            _claimValueProvider = claimValueProvider;
            _logicExecutor = logicExecutor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the command which adds a household to the current users household account.
        /// </summary>
        /// <param name="command">Command for adding a household to the current users household account.</param>
        /// <returns>Service receipt.</returns>
        public virtual ServiceReceiptResponse Execute(HouseholdAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Specification.IsSatisfiedBy(() => CommonValidations.HasValue(command.Name), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Name")))
                .IsSatisfiedBy(() => CommonValidations.IsLengthValid(command.Name, 1, 64), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.LengthForPropertyIsInvalid, "Name", 1, 64)))
                .IsSatisfiedBy(() => CommonValidations.ContainsIllegalChar(command.Name) == false, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "Name")))
                .IsSatisfiedBy(() => command.Description == null || CommonValidations.HasValue(command.Description), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Description")))
                .IsSatisfiedBy(() => command.Description == null || CommonValidations.IsLengthValid(command.Description, 1, 2048), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.LengthForPropertyIsInvalid, "Description", 1, 2048)))
                .IsSatisfiedBy(() => command.Description == null || CommonValidations.ContainsIllegalChar(command.Description) == false, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "Description")))
                .Evaluate();

            return null;
        }

        /// <summary>
        /// Handles an exception which has occurred when executing the command.
        /// </summary>
        /// <param name="command">Command for adding a household to the current users household account.</param>
        /// <param name="exception">Exception.</param>
        public virtual void HandleException(HouseholdAddCommand command, Exception exception)
        {
            throw ExceptionBuilder.Build(exception, MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}
