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
    /// Service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, Namespace = SoapNamespaces.FoodWasteNamespace)]
    [RequiredClaimType(FoodWasteClaimTypes.HouseHoldManagement)]
    [RequiredClaimType(FoodWasteClaimTypes.ValidatedUser)]
    public class FoodWasteHouseholdDataService : IFoodWasteHouseholdDataService
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IFaultExceptionBuilder<FoodWasteFault> _foodWasteFaultExceptionBuilder; 

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a service which can access and modify data on a house hold in the food waste domain.
        /// </summary>
        /// <param name="commandBus">Implementation of the command bus.</param>
        /// <param name="queryBus">Implementation of the query bus.</param>
        /// <param name="foodWasteFaultExceptionBuilder">Implementation of builder which can generate faults.</param>
        public FoodWasteHouseholdDataService(ICommandBus commandBus, IQueryBus queryBus, IFaultExceptionBuilder<FoodWasteFault> foodWasteFaultExceptionBuilder)
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
        public virtual IEnumerable<StorageTypeView> StorageTypeGetAll(StorageTypeCollectionGetQuery query)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets household data for one of the current callers households.
        /// </summary>
        /// <param name="query">Query for getting household data for one of the current callers households.</param>
        /// <returns>Household data.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual HouseholdView HouseholdDataGet(HouseholdDataGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<HouseholdDataGetQuery, HouseholdView>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Adds a new household to the current caller. If the current caller is not created as a household
        /// member account this account would be created.
        /// </summary>
        /// <param name="command">Command for adding a household to the current users household account.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse HouseholdAdd(HouseholdAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                return _commandBus.Publish<HouseholdAddCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Updates a given household on which the current caller has a membership.
        /// </summary>
        /// <param name="command">Command for updatering a household on the current callers household account.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse HouseholdUpdate(HouseholdUpdateCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                return _commandBus.Publish<HouseholdUpdateCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Adds a household member to a given household on the current callers household account.
        /// </summary>
        /// <param name="command">Command for adding a household member to a given household on the current callers household account.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse HouseholdAddHouseholdMember(HouseholdAddHouseholdMemberCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                return _commandBus.Publish<HouseholdAddHouseholdMemberCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Removes a household member from a given household on the current callers household account.
        /// </summary>
        /// <param name="command">Command for removing a household member from a given household on the current callers household account.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse HouseholdRemoveHouseholdMember(HouseholdRemoveHouseholdMemberCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                return _commandBus.Publish<HouseholdRemoveHouseholdMemberCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Gets whether the current caller has been created as a household member.
        /// </summary>
        /// <param name="query">Query which can check whether the current caller has been created as a household member.</param>
        /// <returns>Boolean result.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual BooleanResultResponse HouseholdMemberIsCreated(HouseholdMemberIsCreatedQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<HouseholdMemberIsCreatedQuery, BooleanResultResponse>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Gets whether the current caller has been activated.
        /// </summary>
        /// <param name="query">Query which can check whether the current caller has been activated.</param>
        /// <returns>Boolean result.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual BooleanResultResponse HouseholdMemberIsActivated(HouseholdMemberIsActivatedQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<HouseholdMemberIsActivatedQuery, BooleanResultResponse>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Gets whether the current caller has accepted the privacy policy.
        /// </summary>
        /// <param name="query">Query which can check whether the current caller has accepted the privacy policy.</param>
        /// <returns>Boolean result.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual BooleanResultResponse HouseholdMemberHasAcceptedPrivacyPolicy(HouseholdMemberHasAcceptedPrivacyPolicyQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<HouseholdMemberHasAcceptedPrivacyPolicyQuery, BooleanResultResponse>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Gets household member data for the current caller.
        /// </summary>
        /// <param name="query">Query which can get household member data for the current caller.</param>
        /// <returns>Household member data for the current caller.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual HouseholdMemberView HouseholdMemberDataGet(HouseholdMemberDataGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<HouseholdMemberDataGetQuery, HouseholdMemberView>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Activates the current caller.
        /// </summary>
        /// <param name="command">Command for activating the current callers household member account.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse HouseholdMemberActivate(HouseholdMemberActivateCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                return _commandBus.Publish<HouseholdMemberActivateCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Accepts privacy policy for the current caller.
        /// </summary>
        /// <param name="command">Command for accepting privacy policy on the current callers household member account.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse HouseholdMemberAcceptPrivacyPolicy(HouseholdMemberAcceptPrivacyPolicyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                return _commandBus.Publish<HouseholdMemberAcceptPrivacyPolicyCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Upgrades the membership for the current caller.
        /// </summary>
        /// <param name="command">Command for upgrading the membership on the current callers household member account.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse HouseholdMemberUpgradeMembership(HouseholdMemberUpgradeMembershipCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                return _commandBus.Publish<HouseholdMemberUpgradeMembershipCommand, ServiceReceiptResponse>(command);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Gets the collection of food items.
        /// </summary>
        /// <param name="query">Query for getting the collection of food items.</param>
        /// <returns>Collection of food items.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual FoodItemCollectionView FoodItemCollectionGet(FoodItemCollectionGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<FoodItemCollectionGetQuery, FoodItemCollectionView>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Gets the tree of food groups.
        /// </summary>
        /// <param name="query">Query for getting the tree of food groups.</param>
        /// <returns>Tree of food groups.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual FoodGroupTreeView FoodGroupTreeGet(FoodGroupTreeGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<FoodGroupTreeGetQuery, FoodGroupTreeView>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Gets the privacy policy.
        /// </summary>
        /// <param name="query">Query for getting the privacy policy.</param>
        /// <returns>Privacy policy.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual StaticTextView PrivacyPolicyGet(PrivacyPolicyGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<PrivacyPolicyGetQuery, StaticTextView>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// Gets all the data providers who handles payments.
        /// </summary>
        /// <param name="query">Query for getting a collection of data providers who handles payments.</param>
        /// <returns>Collection of all the data providers who handles payments.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<DataProviderView> DataProviderWhoHandlesPaymentsCollectionGet(DataProviderWhoHandlesPaymentsCollectionGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            try
            {
                return _queryBus.Query<DataProviderWhoHandlesPaymentsCollectionGetQuery, IEnumerable<DataProviderView>>(query);
            }
            catch (Exception ex)
            {
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
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
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion
    }
}