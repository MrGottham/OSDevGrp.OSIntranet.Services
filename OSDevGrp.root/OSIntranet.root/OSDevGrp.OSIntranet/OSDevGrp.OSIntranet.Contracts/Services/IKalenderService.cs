using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til kalender.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.KalenderServiceName, Namespace = SoapNamespaces.IntranetNamespace)]
    public interface IKalenderService : IIntranetService
    {
        /// <summary>
        /// Henter kalenderaftaler til en given kalenderbruger.
        /// </summary>
        /// <param name="query">Forespørgelse efter kalenderaftaler til en given kalenderbruger.</param>
        /// <returns>Liste af kalenderaftaler til den givne kalenderbruger.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<KalenderbrugerAftaleView> KalenderbrugerAftalerGet(KalenderbrugerAftalerGetQuery query);

        /// <summary>
        /// Henter en given kalenderaftale til en given kalenderbruger.
        /// </summary>
        /// <param name="query">Forespørgelse efter en given kalenderaftale til en given kalenderbruger.</param>
        /// <returns>Kalenderaftale til den givne givne kalenderbruger.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        KalenderbrugerAftaleView KalenderbrugerAftaleGet(KalenderbrugerAftaleGetQuery query);

        /// <summary>
        /// Henter alle kalenderbrugere til et givent system under OSWEBDB.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle kalenderbrugere til et givent system under OSWEBDB.</param>
        /// <returns>Liste af alle kalenderbrugere til det givne system under OSWEBDB.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<KalenderbrugerView> KalenderbrugereGet(KalenderbrugereGetQuery query);

        /// <summary>
        /// Henter alle systemer under OSWEBDB.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle systemer under OSWEBDB.</param>
        /// <returns>Liste af alle systemer under OSWEBDB.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<SystemView> SystemerGet(SystemerGetQuery query);
    }
}
