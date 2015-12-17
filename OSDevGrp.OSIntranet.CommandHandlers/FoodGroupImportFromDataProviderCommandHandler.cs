using System;
using System.Reflection;
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
    /// Command handler which handles a command for importing a food group from a given data provider.
    /// </summary>
    public class FoodGroupImportFromDataProviderCommandHandler : FoodWasteSystemDataCommandHandlerBase, ICommandHandler<FoodGroupImportFromDataProviderCommand, ServiceReceiptResponse>
    {
        #region Private variables

        private readonly ILogicExecutor _logicExecutor;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a command handler which handles a command for importing a food group from a given data provider.
        /// </summary>
        /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of the common validations.</param>
        /// <param name="exceptionBuilder">Implementation of the builder which can build exceptions.</param>
        /// <param name="logicExecutor">Implementation of the logic executor which can execute basic logic.</param>
        public FoodGroupImportFromDataProviderCommandHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder, ILogicExecutor logicExecutor)
            : base(systemDataRepository, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
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
        /// Executes the command which handles a command for importing a food group from a given data provider.
        /// </summary>
        /// <param name="command">Command which imports a food group from a given data provider.</param>
        /// <returns>Service receipt.</returns>
        public virtual ServiceReceiptResponse Execute(FoodGroupImportFromDataProviderCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var dataProvider = SystemDataRepository.Get<IDataProvider>(command.DataProviderIdentifier);
            var translationInfo = SystemDataRepository.Get<ITranslationInfo>(command.TranslationInfoIdentifier);

            IFoodGroup parentFoodGroup = null;
            var parentKey = command.ParentKey;
            if (string.IsNullOrWhiteSpace(parentKey) == false)
            {
                parentFoodGroup = SystemDataRepository.FoodGroupGetByForeignKey(dataProvider, parentKey);
            }

            Specification.IsSatisfiedBy(() => CommonValidations.IsNotNull(dataProvider), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, command.DataProviderIdentifier)))
                .IsSatisfiedBy(() => CommonValidations.IsNotNull(translationInfo), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, command.TranslationInfoIdentifier)))
                .IsSatisfiedBy(() => CommonValidations.HasValue(command.Key), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Key")))
                .IsSatisfiedBy(() => CommonValidations.ContainsIllegalChar(command.Key) == false, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "Key")))
                .IsSatisfiedBy(() => CommonValidations.HasValue(command.Name), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Name")))
                .IsSatisfiedBy(() => CommonValidations.ContainsIllegalChar(command.Name) == false, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "Name")))
                .IsSatisfiedBy(() => string.IsNullOrWhiteSpace(parentKey) || (string.IsNullOrWhiteSpace(parentKey) == false && CommonValidations.IsNotNull(parentFoodGroup)), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, parentKey, "ParentKey")))
                .Evaluate();

            var foodGroup = SystemDataRepository.FoodGroupGetByForeignKey(dataProvider, command.Key);
            if (foodGroup == null)
            {
                var insertedfoodGroup = SystemDataRepository.Insert<IFoodGroup>(new FoodGroup {IsActive = true, Parent = parentFoodGroup});

                var foreignKey = new ForeignKey(dataProvider, insertedfoodGroup.Identifier.HasValue ? insertedfoodGroup.Identifier.Value : default(Guid), insertedfoodGroup.GetType(), command.Key);
                foreignKey.Identifier = _logicExecutor.ForeignKeyAdd(foreignKey);
                insertedfoodGroup.ForeignKeyAdd(foreignKey);

                ImportTranslation(insertedfoodGroup, translationInfo, command.Name, _logicExecutor);

                return ObjectMapper.Map<IIdentifiable, ServiceReceiptResponse>(insertedfoodGroup);
            }

            foodGroup.IsActive = command.IsActive;
            foodGroup.Parent = parentFoodGroup;

            var updatedFoodGroup = SystemDataRepository.Update(foodGroup);

            ImportTranslation(updatedFoodGroup, translationInfo, command.Name, _logicExecutor);

            return ObjectMapper.Map<IIdentifiable, ServiceReceiptResponse>(updatedFoodGroup);
        }

        /// <summary>
        /// Handles an exception which has occurred when executing the command.
        /// </summary>
        /// <param name="command">Command which imports a food group from a given data provider.</param>
        /// <param name="exception">Exception.</param>
        public virtual void HandleException(FoodGroupImportFromDataProviderCommand command, Exception exception)
        {
            throw ExceptionBuilder.Build(exception, MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}
