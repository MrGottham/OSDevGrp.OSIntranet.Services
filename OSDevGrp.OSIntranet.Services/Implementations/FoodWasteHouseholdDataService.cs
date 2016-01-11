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
        /// Gets whether the current caller has been created as a household member.
        /// </summary>
        /// <param name="query">Query which can check whether the current caller has been created as a household member.</param>
        /// <returns>Boolean result.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual BooleanResultResponse HouseholdMemberIsCreated(HouseholdMemberIsCreatedQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
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
                throw new ArgumentNullException("query");
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
                throw new ArgumentNullException("query");
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
        /// Activates the current caller.
        /// </summary>
        /// <param name="command">Command for activating the current callers household member account.</param>
        /// <returns>Service receipt.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual ServiceReceiptResponse HouseholdMemberActivate(HouseholdMemberActivateCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
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
                throw new ArgumentNullException("command");
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
        /// Gets the collection of food items.
        /// </summary>
        /// <param name="query">Query for getting the collection of food items.</param>
        /// <returns>Collection of food items.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual FoodItemCollectionView FoodItemCollectionGet(FoodItemCollectionGetQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
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
                throw new ArgumentNullException("query");
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
                throw new ArgumentNullException("query");
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
                throw _foodWasteFaultExceptionBuilder.Build(ex, SoapNamespaces.FoodWasteHouseholdDataServiceName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion
    }
}