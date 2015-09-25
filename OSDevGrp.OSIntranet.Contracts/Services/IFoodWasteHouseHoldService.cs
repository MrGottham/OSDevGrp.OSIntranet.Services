using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts.Faults;

namespace OSDevGrp.OSIntranet.Contracts.Services
{
    /// <summary>
    /// Interface for the service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.HouseHoldServiceName, Namespace = SoapNamespaces.FoodWasteNamespace)]
    public interface IFoodWasteHouseHoldService : IIntranetService
    {
        [OperationContract]
        [FaultContract(typeof (FoodWasteFault))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void Test();
    }
}
