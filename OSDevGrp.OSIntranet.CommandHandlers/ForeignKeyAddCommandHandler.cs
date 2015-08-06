using System;
using System.Linq;
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
    /// Command handler which handles a command for adding a foreign key.
    /// </summary>
    public class ForeignKeyAddCommandHandler : FoodWasteSystemDataCommandHandlerBase, ICommandHandler<ForeignKeyAddCommand, ServiceReceiptResponse>
    {
        #region Constructor

        /// <summary>
        /// Creates a command handler which handles a command for adding a foreign key.
        /// </summary>
        /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of the common validations.</param>
        public ForeignKeyAddCommandHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations)
            : base(systemDataRepository, foodWasteObjectMapper, specification, commonValidations)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the command which adds a foreign key.
        /// </summary>
        /// <param name="command">Command which adds a foreign key.</param>
        /// <returns>Service receipt.</returns>
        public virtual ServiceReceiptResponse Execute(ForeignKeyAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var dataProvider = SystemDataRepository.Get<IDataProvider>(command.DataProviderIdentifier);
            var foreignKeyForDomainObject = GetForeignKeyForDomainObject(command.ForeignKeyForIdentifier);

            Specification.IsSatisfiedBy(() => CommonValidations.IsNotNull(dataProvider), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, command.DataProviderIdentifier)))
                .IsSatisfiedBy(() => CommonValidations.IsNotNull(foreignKeyForDomainObject), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, command.ForeignKeyForIdentifier)))
                .IsSatisfiedBy(() => CommonValidations.HasValue(command.ForeignKeyValue), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "ForeignKeyValue")))
                .IsSatisfiedBy(() => CommonValidations.ContainsIllegalChar(command.ForeignKeyValue) == false, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "ForeignKeyValue")))
                .Evaluate();

            // ReSharper disable PossibleInvalidOperationException
            var foreignKey = SystemDataRepository.Insert<IForeignKey>(new ForeignKey(dataProvider, foreignKeyForDomainObject.Identifier.Value, foreignKeyForDomainObject.GetType(), command.ForeignKeyValue));
            // ReSharper restore PossibleInvalidOperationException

            return ObjectMapper.Map<IIdentifiable, ServiceReceiptResponse>(foreignKey);
        }

        /// <summary>
        /// Handles an exception which has occurred when executing the command.
        /// </summary>
        /// <param name="command">Command which adds a foreign key.</param>
        /// <param name="exception">Exception.</param>
        public virtual void HandleException(ForeignKeyAddCommand command, Exception exception)
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

        /// <summary>
        /// Gets the domain object which has the foreign key.
        /// </summary>
        /// <param name="foreignKeyForIdentifier">Identifier for the domain object which has the foreign key.</param>
        /// <returns>Domain object which has the foreign key.</returns>
        private IForeignKeyable GetForeignKeyForDomainObject(Guid foreignKeyForIdentifier)
        {
            var foreignKeyableInterfaces = typeof (IForeignKeyable).Assembly
                .GetTypes()
                .Where(m => m.IsInterface && m.IsGenericType == false && m.IsPublic && m != typeof (IForeignKeyable) && typeof (IForeignKeyable).IsAssignableFrom(m))
                .ToList();
            foreach (var foreignKeyableInterface in foreignKeyableInterfaces)
            {
                var getMethod = typeof (IDataRepository).GetMethod("Get", new[] {typeof (Guid)}).MakeGenericMethod(new[] {foreignKeyableInterface});
                try
                {
                    var foreignKeyableDomainObject = getMethod.Invoke(SystemDataRepository, new object[] {foreignKeyForIdentifier}) as IForeignKeyable;
                    if (foreignKeyableDomainObject != null)
                    {
                        return foreignKeyableDomainObject;
                    }
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException is IntranetRepositoryException)
                    {
                        continue;
                    }
                    throw;
                }
                catch (IntranetRepositoryException)
                {
                }
            }
            return null;
        }

        #endregion
    }
}
