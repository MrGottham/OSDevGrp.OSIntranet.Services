﻿using System;
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
    [RequiredClaimType(FoodWasteClaimTypes.ValidatedUser)]
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
            _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
            _queryBus = queryBus ?? throw new ArgumentNullException(nameof(queryBus));
            _foodWasteFaultExceptionBuilder = foodWasteFaultExceptionBuilder ?? throw new ArgumentNullException(nameof(foodWasteFaultExceptionBuilder));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all the storage types.
        /// </summary>
        /// <param name="query">Query for getting all the storage types.</param>
        /// <returns>Collection of all the storage types.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual IEnumerable<StorageTypeSystemView> StorageTypeGetAll(StorageTypeCollectionGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<StorageTypeCollectionGetQuery, IEnumerable<StorageTypeSystemView>>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Gets the collection of food items.
        /// </summary>
        /// <param name="query">Query for getting the collection of food items.</param>
        /// <returns>Collection of food items.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual FoodItemCollectionSystemView FoodItemCollectionGet(FoodItemCollectionGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<FoodItemCollectionGetQuery, FoodItemCollectionSystemView>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Imports a food item from a given data provider.
        /// </summary>
        /// <param name="command">Command for importing a food item from a given data provider.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse FoodItemImportFromDataProvider(FoodItemImportFromDataProviderCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                return _commandBus.Publish<FoodItemImportFromDataProviderCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Gets the tree of food groups.
        /// </summary>
        /// <param name="query">Query for getting the tree of food groups.</param>
        /// <returns>Tree of food groups.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual FoodGroupTreeSystemView FoodGroupTreeGet(FoodGroupTreeGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<FoodGroupTreeGetQuery, FoodGroupTreeSystemView>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Imports a food group from a given data provider.
        /// </summary>
        /// <param name="command">Command for importing a food group from a given data provider.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse FoodGroupImportFromDataProvider(FoodGroupImportFromDataProviderCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                return _commandBus.Publish<FoodGroupImportFromDataProviderCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Adds a dataproviders foreign key to a given domain object in the food waste domain.
        /// </summary>
        /// <param name="command">Command for adding a dataproviders foreign key to a given domain object in the food waste domain.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse ForeignKeyAdd(ForeignKeyAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                return _commandBus.Publish<ForeignKeyAddCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Modifies a dataproviders foreign key to a given domain object in the food waste domain.
        /// </summary>
        /// <param name="command">Command for modifying a dataproviders foreign key to a given domain object in the food waste domain.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse ForeignKeyModify(ForeignKeyModifyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                return _commandBus.Publish<ForeignKeyModifyCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Deletes a dataproviders foreign key to a given domain object in the food waste domain.
        /// </summary>
        /// <param name="command">Command for deleting a dataproviders foreign key to a given domain object in the food waste domain.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse ForeignKeyDelete(ForeignKeyDeleteCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                return _commandBus.Publish<ForeignKeyDeleteCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Gets all the data providers.
        /// </summary>
        /// <param name="query">Query for getting all the data providers.</param>
        /// <returns>Collection of all the data providers.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual IEnumerable<DataProviderSystemView> DataProviderGetAll(DataProviderCollectionGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<DataProviderCollectionGetQuery, IEnumerable<DataProviderSystemView>>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteSystemDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

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
                throw new ArgumentNullException(nameof(command));
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
                throw new ArgumentNullException(nameof(command));
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
                throw new ArgumentNullException(nameof(command));
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
        /// Gets all the static texts.
        /// </summary>
        /// <param name="query">Query for getting all the static texts.</param>
        /// <returns>Collection of all the static texts.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual IEnumerable<StaticTextSystemView> StaticTextGetAll(StaticTextCollectionGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<StaticTextCollectionGetQuery, IEnumerable<StaticTextSystemView>>(query);
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
                throw new ArgumentNullException(nameof(query));
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