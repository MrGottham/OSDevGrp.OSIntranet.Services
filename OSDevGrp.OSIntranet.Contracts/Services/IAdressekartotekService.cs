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
        /// Henter en telefonliste.
        /// </summary>
        /// <param name="query">Forespørgelse efter en telefonliste.</param>
        /// <returns>Telefonliste.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<TelefonlisteView> TelefonlisteGet(TelefonlisteGetQuery query);

        /// <summary>
        /// Henter en liste af personer.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af personer.</param>
        /// <returns>Liste af personer.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<PersonView> PersonlisteGet(PersonlisteGetQuery query);

        /// <summary>
        /// Henter en given person.
        /// </summary>
        /// <param name="query">Forespørgelse efter en given person.</param>
        /// <returns>Person.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        PersonView PersonGet(PersonGetQuery query);

        /// <summary>
        /// Henter en liste af firmaer.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af firmaer.</param>
        /// <returns>Liste af firmaer.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<FirmaView> FirmalisteGet(FirmalisteGetQuery query);

        /// <summary>
        /// Henter et givent firma.
        /// </summary>
        /// <param name="query">Forespørgelse efter et givent firma.</param>
        /// <returns>Firma.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        FirmaView FirmaGet(FirmaGetQuery query);

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
