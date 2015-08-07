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
    /// Interface for the service which can access and modify system data in the food waste domain.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.FoodWasteSystemDataServiceName, Namespace = SoapNamespaces.FoodWasteNamespace)]
    public interface IFoodWasteSystemDataService : IIntranetService
    {
        /// <summary>
        /// Adds a dataproviders foreign key to a given domain object in the food waste domain.
        /// </summary>
        /// <param name="command">Command for adding a dataproviders foreign key to a given domain object in the food waste domain.</param>
        /// <returns>Service receipt.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        ServiceReceiptResponse ForeignKeyAdd(ForeignKeyAddCommand command);

        /// <summary>
        /// Modifies a dataproviders foreign key to a given domain object in the food waste domain.
        /// </summary>
        /// <param name="command">Command for modifying a dataproviders foreign key to a given domain object in the food waste domain.</param>
        /// <returns>Service receipt.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        ServiceReceiptResponse ForeignKeyModify(ForeignKeyModifyCommand command);

        /// <summary>
        /// Deletes a dataproviders foreign key to a given domain object in the food waste domain.
        /// </summary>
        /// <param name="command">Command for deleting a dataproviders foreign key to a given domain object in the food waste domain.</param>
        /// <returns>Service receipt.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        ServiceReceiptResponse ForeignKeyDelete(ForeignKeyDeleteCommand command);

        /// <summary>
        /// Gets all the data providers.
        /// </summary>
        /// <param name="query">Query for getting all the data providers.</param>
        /// <returns>Collection of all the data providers.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<DataProviderSystemView> DataProviderGetAll(DataProviderCollectionGetQuery query);

        /// <summary>
        /// Adds a translation.
        /// </summary>
        /// <param name="command">Command for adding a translation.</param>
        /// <returns>Service receipt.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        ServiceReceiptResponse TranslationAdd(TranslationAddCommand command);

        /// <summary>
        /// Modify a translation.
        /// </summary>
        /// <param name="command">Command for modifying a translation.</param>
        /// <returns>Service receipt.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        ServiceReceiptResponse TranslationModify(TranslationModifyCommand command);

        /// <summary>
        /// Delete a translation.
        /// </summary>
        /// <param name="command">Command for deleting a translation.</param>
        /// <returns>Service receipt.</returns>
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        ServiceReceiptResponse TranslationDelete(TranslationDeleteCommand command);

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
