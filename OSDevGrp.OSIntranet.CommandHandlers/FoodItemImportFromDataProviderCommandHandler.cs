﻿using System;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
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

            return null;
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
