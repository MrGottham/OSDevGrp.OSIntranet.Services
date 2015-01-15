using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Contracts.Services
{
    /// <summary>
    /// Interface for the service which can access and modify system data in the food waste domain.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.FoodWasteSystemDataServiceName, Namespace = SoapNamespaces.FoodWasteNamespace)]
    public interface IFoodWasteSystemDataService : IIntranetService
    {
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
