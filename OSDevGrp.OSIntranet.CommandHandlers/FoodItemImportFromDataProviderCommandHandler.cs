using System;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers
{
    /// <summary>
    /// Command handler which handles a command for importing a food item from a given data provider.
    /// </summary>
    public class FoodItemImportFromDataProviderCommandHandler : FoodWasteSystemDataCommandHandlerBase, ICommandHandler<FoodItemImportFromDataProviderCommand, ServiceReceiptResponse>
    {
        #region Private variables

        private readonly ILogicExecutor _logicExecutor;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a command handler which handles a command for importing a food item from a given data provider.
        /// </summary>
        /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of the common validations.</param>
        /// <param name="logicExecutor">Implementation of the logic executor which can execute basic logic.</param>
        public FoodItemImportFromDataProviderCommandHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, ILogicExecutor logicExecutor)
            : base(systemDataRepository, foodWasteObjectMapper, specification, commonValidations)
        {
            if (logicExecutor == null)
            {
                throw new ArgumentNullException("logicExecutor");
            }
            _logicExecutor = logicExecutor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the command which handles a command for importing a food item from a given data provider.
        /// </summary>
        /// <param name="command">Command which imports a food item from a given data provider.</param>
        /// <returns>Service receipt.</returns>
        public virtual ServiceReceiptResponse Execute(FoodItemImportFromDataProviderCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var dataProvider = SystemDataRepository.Get<IDataProvider>(command.DataProviderIdentifier);
            var translationInfo = SystemDataRepository.Get<ITranslationInfo>(command.TranslationInfoIdentifier);
            var primaryFoodGroup = SystemDataRepository.Get<IFoodGroup>(command.PrimaryFoodGroupIdentifier);

            Specification.IsSatisfiedBy(() => CommonValidations.IsNotNull(dataProvider), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, command.DataProviderIdentifier)))
                .IsSatisfiedBy(() => CommonValidations.IsNotNull(translationInfo), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, command.TranslationInfoIdentifier)))
                .IsSatisfiedBy(() => CommonValidations.IsNotNull(primaryFoodGroup), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, command.PrimaryFoodGroupIdentifier)))
                .IsSatisfiedBy(() => CommonValidations.HasValue(command.Key), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Key")))
                .IsSatisfiedBy(() => CommonValidations.ContainsIllegalChar(command.Key), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "Key")))
                .IsSatisfiedBy(() => CommonValidations.HasValue(command.Name), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Name")))
                .IsSatisfiedBy(() => CommonValidations.ContainsIllegalChar(command.Name), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "Name")))
                .Evaluate();

            var foodItem = SystemDataRepository.FoodItemGetByForeignKey(dataProvider, command.Key);
            if (foodItem == null)
            {
                var insertedFoodItem = SystemDataRepository.Insert<IFoodItem>(new FoodItem(primaryFoodGroup) {IsActive = command.IsActive});

                var foreignKey = new ForeignKey(dataProvider, insertedFoodItem.Identifier.HasValue ? insertedFoodItem.Identifier.Value : default(Guid), insertedFoodItem.GetType(), command.Key);
                foreignKey.Identifier = _logicExecutor.ForeignKeyAdd(foreignKey);
                insertedFoodItem.ForeignKeyAdd(foreignKey);

                ImportTranslation(insertedFoodItem, translationInfo, command.Name, _logicExecutor);
                
                return ObjectMapper.Map<IIdentifiable, ServiceReceiptResponse>(insertedFoodItem);
            }

            foodItem.IsActive = command.IsActive;

            var updatedFoodItem = SystemDataRepository.Update(foodItem);

            ImportTranslation(updatedFoodItem, translationInfo, command.Name, _logicExecutor);

            return ObjectMapper.Map<IIdentifiable, ServiceReceiptResponse>(updatedFoodItem);
        }

        /// <summary>
        /// Handles an exception which has occurred when executing the command.
        /// </summary>
        /// <param name="command">Command which imports a food item from a given data provider.</param>
        /// <param name="exception">Exception</param>
        public virtual void HandleException(FoodItemImportFromDataProviderCommand command, Exception exception)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (exception.GetType() == typeof (IntranetRepositoryException) || exception.GetType() == typeof (IntranetBusinessException) || exception.GetType() == typeof (IntranetSystemException))
            {
                throw exception;
            }
            throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue, command.GetType().Name, typeof (ServiceReceiptResponse).Name, exception.Message), exception);
        }

        #endregion
    }
}
