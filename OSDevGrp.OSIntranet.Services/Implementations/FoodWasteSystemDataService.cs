using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Security.Attributes;
using OSDevGrp.OSIntranet.Security.Claims;

namespace OSDevGrp.OSIntranet.Services.Implementations
{
    /// <summary>
    /// Service which can access and modify system data in the food waste domain.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, Namespace = SoapNamespaces.FoodWasteNamespace)]
    [RequiredClaimType(FoodWasteClaimTypes.SystemManagement)]
    public class FoodWasteSystemDataService : IFoodWasteSystemDataService
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IFaultExceptionBuilder<FoodWasteFault> _foodWasteFaultExceptionBuilder; 

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a service which can access and modify system data in the food waste domain.
        /// </summary>
        /// <param name="commandBus">Implementation of the command bus.</param>
        /// <param name="queryBus">Implementation of the query bus.</param>
        /// <param name="foodWasteFaultExceptionBuilder">Implementation of builder which can generate faults.</param>
        public FoodWasteSystemDataService(ICommandBus commandBus, IQueryBus queryBus, IFaultExceptionBuilder<FoodWasteFault> foodWasteFaultExceptionBuilder)
        {
            if (commandBus == null)
            {
                throw new ArgumentNullException("commandBus");
            }
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            if (foodWasteFaultExceptionBuilder == null)
            {
                throw new ArgumentNullException("foodWasteFaultExceptionBuilder");
            }
            _commandBus = commandBus;
            _queryBus = queryBus;
            _foodWasteFaultExceptionBuilder = foodWasteFaultExceptionBuilder;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a translation.
        /// </summary>
        /// <param name="command">Command for adding a translation.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse TranslationAdd(TranslationAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            try
            {
                return _commandBus.Publish<TranslationAddCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Modify a translation.
        /// </summary>
        /// <param name="command">Command for modifying a translation.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse TranslationModify(TranslationModifyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            try
            {
                return _commandBus.Publish<TranslationModifyCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Delete a translation.
        /// </summary>
        /// <param name="command">Command for deleting a translation.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse TranslationDelete(TranslationDeleteCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            try
            {
                return _commandBus.Publish<TranslationDeleteCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Gets all the translation informations which can be used for translations.
        /// </summary>
        /// <param name="query">Query for getting all the translation informations which can be used for translations.</param>
        /// <returns>Collection of all the translation informations which can be used for translations.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual IEnumerable<TranslationInfoSystemView> TranslationInfoGetAll(TranslationInfoCollectionGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            try
            {
                return _queryBus.Query<TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion
    }
}