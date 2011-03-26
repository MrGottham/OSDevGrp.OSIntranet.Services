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

        /// <summary>
        /// Henter en kontoplan.
        /// </summary>
        /// <param name="query">Forespørgelse efter en kontoplan.</param>
        /// <returns>Kontoplan.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<KontoplanView> KontoplanGet(KontoplanGetQuery query);

        /// <summary>
        /// Henter en konto.
        /// </summary>
        /// <param name="query">Forespørgelse efter en konto.</param>
        /// <returns>Konto.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        KontoView KontoGet(KontoGetQuery query);

        /// <summary>
        /// Henter en budgetkontoplan.
        /// </summary>
        /// <param name="query">Forespørgelse efter en budgetkontoplan.</param>
        /// <returns>Budgetkontoplan.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<BudgetkontoplanView> BudgetkontoplanGet(BudgetkontoplanGetQuery query);

        /// <summary>
        /// Henter en budgetkonto.
        /// </summary>
        /// <param name="query">Forespørgelse efter en budgetkonto.</param>
        /// <returns>Budgetkonto.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BudgetkontoView BudgetkontoGet(BudgetkontoGetQuery query);

        /// <summary>
        /// Henter en liste af debitorer.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af debitorer.</param>
        /// <returns>Liste af debitorer.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<DebitorlisteView> DebitorlisteGet(DebitorlisteGetQuery query);

        /// <summary>
        /// Henter en debitor.
        /// </summary>
        /// <param name="query">Forespørgelse efter en debitor.</param>
        /// <returns>Debitor.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        DebitorView DebitorGet(DebitorGetQuery query);

        /// <summary>
        /// Henter en liste af kreditorer.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af kreditorer.</param>
        /// <returns>Liste af kreditorer.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<KreditorlisteView> KreditorlisteGet(KreditorlisteGetQuery query);

        /// <summary>
        /// Henter en kreditor.
        /// </summary>
        /// <param name="query">Forespørgelse efter en kreditor.</param>
        /// <returns>Kreditor</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        KreditorView KreditorGet(KreditorGetQuery query);

        /// <summary>
        /// Hente en liste af adressekonti.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af adressekonti.</param>
        /// <returns>Liste af adressekonti.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<AdressekontolisteView> AdressekontolisteGet(AdressekontolisteGetQuery query);

        /// <summary>
        /// Hente en adressekonto.
        /// </summary>
        /// <param name="query">Forespørgelse efter en adressekonto.</param>
        /// <returns>Liste af adressekonto.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        AdressekontoView AdressekontoGet(AdressekontoGetQuery query);


        // TODO: Lav metode, der kan hente et givent antal bogføringslinjer.

        // TODO: Lav metode, der kan hente betalingsbetingelser.
        // TODO: Lav metode, der kan hente kontogrupper.
        // TODO: Lav metode, der kan hente budgetkontogrupper.
    }
}
