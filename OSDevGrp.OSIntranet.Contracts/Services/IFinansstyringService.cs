using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til finansstyring.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.FinansstyringServiceName, Namespace = SoapNamespaces.IntranetNamespace)]
    public interface IFinansstyringService : IIntranetService
    {
        /// <summary>
        /// Henter en regnskabsliste.
        /// </summary>
        /// <param name="query">Forespørgelse efter en regnskabsliste.</param>
        /// <returns>Regnskabsliste.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<RegnskabslisteView> RegnskabslisteGet(RegnskabslisteGetQuery query);
    }
}
