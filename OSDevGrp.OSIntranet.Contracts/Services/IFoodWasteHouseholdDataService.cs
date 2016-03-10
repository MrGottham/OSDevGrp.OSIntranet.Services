using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Contracts.Services
{
    /// <summary>
    /// Interface for the service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.FoodWasteHouseholdDataServiceName, Namespace = SoapNamespaces.FoodWasteNamespace)]
    public interface IFoodWasteHouseholdDataService : IIntranetService
    {
        /// <summary>
        /// Gets household data for one of the current callers households.
        /// </summary>
        /// <param name="query">Query for getting household data for one of the current callers households.</param>
        /// <returns>Household data.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        HouseholdView HouseholdDataGet(HouseholdDataGetQuery query);

        /// <summary>
        /// Adds a new household to the current caller. If the current caller is not created as a household
        /// member account this account would be created.
        /// </summary>
        /// <param name="command">Command for adding a household to the current users household account.</param>
        /// <returns>Service receipt.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        ServiceReceiptResponse HouseholdAdd(HouseholdAddCommand command);

        /// <summary>
        /// Gets whether the current caller has been created as a household member.
        /// </summary>
        /// <param name="query">Query which can check whether the current caller has been created as a household member.</param>
        /// <returns>Boolean result.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BooleanResultResponse HouseholdMemberIsCreated(HouseholdMemberIsCreatedQuery query);

        /// <summary>
        /// Gets whether the current caller has been activated.
        /// </summary>
        /// <param name="query">Query which can check whether the current caller has been activated.</param>
        /// <returns>Boolean result.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BooleanResultResponse HouseholdMemberIsActivated(HouseholdMemberIsActivatedQuery query);

        /// <summary>
        /// Gets whether the current caller has accepted the privacy policy.
        /// </summary>
        /// <param name="query">Query which can check whether the current caller has accepted the privacy policy.</param>
        /// <returns>Boolean result.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BooleanResultResponse HouseholdMemberHasAcceptedPrivacyPolicy(HouseholdMemberHasAcceptedPrivacyPolicyQuery query);

        /// <summary>
        /// Gets household member data for the current caller.
        /// </summary>
        /// <param name="query">Query which can get household member data for the current caller.</param>
        /// <returns>Household member data for the current caller.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        HouseholdMemberView HouseholdMemberDataGet(HouseholdMemberDataGetQuery query);

        /// <summary>
        /// Activates the current caller.
        /// </summary>
        /// <param name="command">Command for activating the current callers household member account.</param>
        /// <returns>Service receipt.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        ServiceReceiptResponse HouseholdMemberActivate(HouseholdMemberActivateCommand command);

        /// <summary>
        /// Accepts privacy policy for the current caller.
        /// </summary>
        /// <param name="command">Command for accepting privacy policy on the current callers household member account.</param>
        /// <returns>Service receipt.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        ServiceReceiptResponse HouseholdMemberAcceptPrivacyPolicy(HouseholdMemberAcceptPrivacyPolicyCommand command);

        /// <summary>
        /// Upgrades the membership for the current caller.
        /// </summary>
        /// <param name="command">Command for upgrading the membership on the current callers household member account.</param>
        /// <returns>Service receipt.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        ServiceReceiptResponse HouseholdMemberUpgradeMembership(HouseholdMemberUpgradeMembershipCommand command);

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
        /// Gets all the data providers who handles payments.
        /// </summary>
        /// <param name="query">Query for getting a collection of data providers who handles payments.</param>
        /// <returns>Collection of all the data providers who handles payments.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<DataProviderView> DataProviderWhoHandlesPaymentsCollectionGet(DataProviderWhoHandlesPaymentsCollectionGetQuery query);

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
