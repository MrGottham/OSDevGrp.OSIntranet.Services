using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til fælles elementer.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.CommonServiceName, Namespace = SoapNamespaces.IntranetNamespace)]
    public interface ICommonService : IIntranetService
    {
        /// <summary>
        /// Henter alle brevhoveder.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle brevhoveder.</param>
        /// <returns>Liste af alle brevhoveder.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<BrevhovedView> BrevhovederGet(BrevhovederGetQuery query);

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
