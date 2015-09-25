using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Contracts.Services
{
    /// <summary>
    /// Interface for the service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.FoodWasteHouseHoldServiceName, Namespace = SoapNamespaces.FoodWasteNamespace)]
    public interface IFoodWasteHouseHoldService : IIntranetService
    {
        /// <summary>
        /// Gets the tree of food groups.
        /// </summary>
        /// <param name="query">Query for getting the tree of food groups.</param>
        /// <returns>Tree of food groups.</returns>
        [OperationContract]
        [FaultContract(typeof(FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        FoodGroupTreeView FoodGroupTreeGet(FoodGroupTreeGetQuery query);

        /// <summary>
        /// Gets all the translation informations which can be used for translations.
        /// </summary>
        /// <param name="query">Query for getting all the translation informations which can be used for translations.</param>
        /// <returns>Collection of all the translation informations which can be used for translations.</returns>
        [OperationContract]
        [FaultContract(typeof(FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<TranslationInfoSystemView> TranslationInfoGetAll(TranslationInfoCollectionGetQuery query);
    }
}
