﻿using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Contracts.Services
{
    /// <summary>
    /// Interface for the service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.FoodWasteHouseholdServiceName, Namespace = SoapNamespaces.FoodWasteNamespace)]
    public interface IFoodWasteHouseholdService : IIntranetService
    {
        /// <summary>
        /// Gets whether the current caller has been created as a household member.
        /// </summary>
        /// <param name="query">Query which can check whether the caller caller has been created as a household member.</param>
        /// <returns>Boolean result.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BooleanResultResponse HouseholdMemberIsCreated(HouseholdMemberIsCreatedQuery query);

        /// <summary>
        /// Gets whether the current caller has been activated.
        /// </summary>
        /// <param name="query">Query which can check whether the current user has been activated.</param>
        /// <returns>Boolean result.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BooleanResultResponse HouseholdMemberIsActivated(HouseholdMemberIsActivatedQuery query);

        /// <summary>
        /// Gets the collection of food items.
        /// </summary>
        /// <param name="query">Query for getting the collection of food items.</param>
        /// <returns>Collection of food items.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        FoodItemCollectionView FoodItemCollectionGet(FoodItemCollectionGetQuery query);

        /// <summary>
        /// Gets the tree of food groups.
        /// </summary>
        /// <param name="query">Query for getting the tree of food groups.</param>
        /// <returns>Tree of food groups.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        FoodGroupTreeView FoodGroupTreeGet(FoodGroupTreeGetQuery query);

        /// <summary>
        /// Gets the privacy policy.
        /// </summary>
        /// <param name="query">Query for getting the privacy policy.</param>
        /// <returns>Privacy policy.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        StaticTextView PrivacyPolicyGet(PrivacyPolicyGetQuery query);

        /// <summary>
        /// Gets all the translation informations which can be used for translations.
        /// </summary>
        /// <param name="query">Query for getting all the translation informations which can be used for translations.</param>
        /// <returns>Collection of all the translation informations which can be used for translations.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<TranslationInfoSystemView> TranslationInfoGetAll(TranslationInfoCollectionGetQuery query);
    }
}
