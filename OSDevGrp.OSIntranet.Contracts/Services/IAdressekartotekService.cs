using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til adressekartotek.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.AdressekartotekServiceName, Namespace = SoapNamespaces.IntranetNamespace)]
    public interface IAdressekartotekService : IIntranetService
    {
        /// <summary>
        /// Henter alle postnumre.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle postnumre.</param>
        /// <returns>Liste af postnumre.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<PostnummerView> PostnumreGet(PostnumreGetQuery query);

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <param name="query">Foresprøgelse efter alle adressegrupper.</param>
        /// <returns>Liste af adressegrupper.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<AdressegruppeView> AdressegrupperGet(AdressegrupperGetQuery query);

        /// <summary>
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <param name="query">Foresprøgelse efter alle betalingsbetingelser.</param>
        /// <returns>Liste af betalingsbetingelser.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<BetalingsbetingelseView> BetalingsbetingelserGet(BetalingsbetingelserGetQuery query);
    }
}
