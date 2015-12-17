using System;
using System.Reflection;
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
    /// Command handler which handles a command for deleting a translation.
    /// </summary>
    public class TranslationDeleteCommandHandler : FoodWasteSystemDataCommandHandlerBase, ICommandHandler<TranslationDeleteCommand, ServiceReceiptResponse>
    {
        #region Constructor

        /// <summary>
        /// Creates a command handler which handles a command for deleting a translation.
        /// </summary>
        /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of the common validations.</param>
        /// <param name="exceptionBuilder">Implementation of the builder which can build exceptions.</param>
        public TranslationDeleteCommandHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder)
            : base(systemDataRepository, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the command which deletes a translation.
        /// </summary>
        /// <param name="command">Command for deleting a translation.</param>
        /// <returns>Service receipt.</returns>
        public virtual ServiceReceiptResponse Execute(TranslationDeleteCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var translation = SystemDataRepository.Get<ITranslation>(command.TranslationIdentifier);

            Specification.IsSatisfiedBy(() => CommonValidations.IsNotNull(translation), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, command.TranslationIdentifier)))
                .Evaluate();

            SystemDataRepository.Delete(translation);

            return ObjectMapper.Map<IIdentifiable, ServiceReceiptResponse>(translation);
        }

        /// <summary>
        /// Handles an exception which has occurred when executing the command.
        /// </summary>
        /// <param name="command">Command for deleting a translation.</param>
        /// <param name="exception">Exception.</param>
        public virtual void HandleException(TranslationDeleteCommand command, Exception exception)
        {
            throw ExceptionBuilder.Build(exception, MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}
