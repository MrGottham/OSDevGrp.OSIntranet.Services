using System;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers
{
    /// <summary>
    /// Command handler which handles a command for adding a translation.
    /// </summary>
    public class TranslationAddCommandHandler : FoodWasteSystemDataCommandHandlerBase, ICommandHandler<TranslationAddCommand, ServiceReceiptResponse>
    {
        #region Constructor

        /// <summary>
        /// Creates a command handler which handles a command for adding a translation.
        /// </summary>
        /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of the common validations.</param>
        public TranslationAddCommandHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations)
            : base(systemDataRepository, foodWasteObjectMapper, specification, commonValidations)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the command which adds a translation.
        /// </summary>
        /// <param name="command">Command for adding a translation.</param>
        /// <returns>Service receipt.</returns>
        public virtual ServiceReceiptResponse Execute(TranslationAddCommand command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles an exception which has occurred when executing the command.
        /// </summary>
        /// <param name="command">Command for adding a translation.</param>
        /// <param name="exception">Exception.</param>
        public virtual void HandleException(TranslationAddCommand command, Exception exception)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
