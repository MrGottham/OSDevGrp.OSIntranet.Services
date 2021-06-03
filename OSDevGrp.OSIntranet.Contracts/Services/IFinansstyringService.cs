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
    /// Servicekontrakt til finansstyring.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.FinansstyringServiceName, Namespace = SoapNamespaces.IntranetNamespace)]
    public interface IFinansstyringService : IIntranetService
    {
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
        /// Henter et givent antal bogføringslinjer fra en given statusdato.
        /// </summary>
        /// <param name="query">Forespørgelse efter et givent antal bogføringslinjer.</param>
        /// <returns>Liste af bogføringslinjer.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<BogføringslinjeView> BogføringerGet(BogføringerGetQuery query);

        /// <summary>
        /// Opretter en bogføringslinje.
        /// </summary>
        /// <param name="command">Kommando til oprettelse af en bogføringslinje.</param>
        /// <returns>Svar fra oprettelse af en bogføringslinje.</returns>
        [OperationContract]
        [FaultContract(typeof(IntranetFaultBase))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BogføringslinjeOpretResponse BogføringslinjeOpret(BogføringslinjeOpretCommand command);
    }
}