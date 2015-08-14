using System;
using System.Reflection;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Logic executor which can execute basic logic.
    /// </summary>
    public class LogicExecuter : ILogicExecuter
    {
        #region Private variables

        private readonly ICommandBus _commandBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a logic executor which can execute basic logic.
        /// </summary>
        /// <param name="commandBus">Implementation of a command bus which can publish commands.</param>
        public LogicExecuter(ICommandBus commandBus)
        {
            if (commandBus == null)
            {
                throw new ArgumentNullException("commandBus");
            }
            _commandBus = commandBus;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes functionality which adds a foreign key to a domain object.
        /// </summary>
        /// <param name="foreignKey">Foreign key to add.</param>
        /// <returns>Identifier for the added foreign key.</returns>
        public virtual Guid ForeignKeyAdd(IForeignKey foreignKey)
        {
            if (foreignKey == null)
            {
                throw new ArgumentNullException("foreignKey");
            }
            var command = new ForeignKeyAddCommand
            {
                DataProviderIdentifier = foreignKey.DataProvider.Identifier.HasValue ? foreignKey.DataProvider.Identifier.Value : default(Guid),
                ForeignKeyForIdentifier = foreignKey.ForeignKeyForIdentifier,
                ForeignKeyValue = foreignKey.ForeignKeyValue
            };
            var serviceReceipt = Execute<ForeignKeyAddCommand, ServiceReceiptResponse>(command);
            return serviceReceipt.Identifier.HasValue ? serviceReceipt.Identifier.Value : default(Guid);
        }

        /// <summary>
        /// Executes functionality which modifies a foreign key for a domain object.
        /// </summary>
        /// <param name="foreignKey">Foreign key to modify.</param>
        /// <returns>Identifier for the modified foreign key.</returns>
        public virtual Guid ForeignKeyModify(IForeignKey foreignKey)
        {
            if (foreignKey == null)
            {
                throw new ArgumentNullException("foreignKey");
            }
            if (foreignKey.Identifier.HasValue == false)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foreignKey.Identifier, "Identifier"));
            }
            var command = new ForeignKeyModifyCommand
            {
                ForeignKeyIdentifier = foreignKey.Identifier.Value,
                ForeignKeyValue = foreignKey.ForeignKeyValue
            };
            var serviceReceipt = Execute<ForeignKeyModifyCommand, ServiceReceiptResponse>(command);
            return serviceReceipt.Identifier.HasValue ? serviceReceipt.Identifier.Value : default(Guid);
        }

        /// <summary>
        /// Executes functionality which delete a foreign key for a domain object.
        /// </summary>
        /// <param name="foreignKey">Foreign key to delete.</param>
        /// <returns>Identifier for the deleted foreign key.</returns>
        public virtual Guid ForeignKeyDelete(IForeignKey foreignKey)
        {
            if (foreignKey == null)
            {
                throw new ArgumentNullException("foreignKey");
            }
            if (foreignKey.Identifier.HasValue == false)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foreignKey.Identifier, "Identifier"));
            }
            var command = new ForeignKeyDeleteCommand
            {
                ForeignKeyIdentifier = foreignKey.Identifier.Value
            };
            var serviceReceipt = Execute<ForeignKeyDeleteCommand, ServiceReceiptResponse>(command);
            return serviceReceipt.Identifier.HasValue ? serviceReceipt.Identifier.Value : default(Guid);
        }

        /// <summary>
        /// Execute functionality which adds a translation.
        /// </summary>
        /// <param name="translation">Translation to add.</param>
        /// <returns>Identifier for the added translation.</returns>
        public virtual Guid TranslationAdd(ITranslation translation)
        {
            if (translation == null)
            {
                throw new ArgumentNullException("translation");
            }
            var command = new TranslationAddCommand
            {
                TranslationInfoIdentifier = translation.TranslationInfo.Identifier.HasValue ? translation.TranslationInfo.Identifier.Value : default(Guid),
                TranslationOfIdentifier = translation.TranslationOfIdentifier,
                TranslationValue = translation.Value
            };
            var serviceReceipt = Execute<TranslationAddCommand, ServiceReceiptResponse>(command);
            return serviceReceipt.Identifier.HasValue ? serviceReceipt.Identifier.Value : default(Guid);
        }

        /// <summary>
        /// Execute functionality which modifies a translation.
        /// </summary>
        /// <param name="translation">Translation to modify.</param>
        /// <returns>Identifier for the modified translation.</returns>
        public virtual Guid TranslationModify(ITranslation translation)
        {
            if (translation == null)
            {
                throw new ArgumentNullException("translation");
            }
            if (translation.Identifier.HasValue == false)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, translation.Identifier, "Identifier"));
            }
            var command = new TranslationModifyCommand
            {
                TranslationIdentifier = translation.Identifier.Value,
                TranslationValue = translation.Value
            };
            var serviceReceipt = Execute<TranslationModifyCommand, ServiceReceiptResponse>(command);
            return serviceReceipt.Identifier.HasValue ? serviceReceipt.Identifier.Value : default(Guid);
        }

        /// <summary>
        /// Execute functionality which deletes a translation.
        /// </summary>
        /// <param name="translation">Translation to delete.</param>
        /// <returns>Identifier for the deleted translation.</returns>
        public virtual Guid TranslationDelete(ITranslation translation)
        {
            if (translation == null)
            {
                throw new ArgumentNullException("translation");
            }
            if (translation.Identifier.HasValue == false)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, translation.Identifier, "Identifier"));
            }
            var command = new TranslationDeleteCommand
            {
                TranslationIdentifier = translation.Identifier.Value
            };
            var serviceReceipt = Execute<TranslationDeleteCommand, ServiceReceiptResponse>(command);
            return serviceReceipt.Identifier.HasValue ? serviceReceipt.Identifier.Value : default(Guid);
        }

        /// <summary>
        /// Executes functionality for a given command.
        /// </summary>
        /// <typeparam name="TCommand">Type of the command on which to execute functionality.</typeparam>
        /// <typeparam name="TResponse">Type of the response expected for the command.</typeparam>
        /// <param name="command">Command.</param>
        /// <returns>Response.</returns>
        private TResponse Execute<TCommand, TResponse>(TCommand command) where TCommand : class, ICommand
        {
            try
            {
                return _commandBus.Publish<TCommand, TResponse>(command);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                throw;
            }
        }

        #endregion
    }
}
