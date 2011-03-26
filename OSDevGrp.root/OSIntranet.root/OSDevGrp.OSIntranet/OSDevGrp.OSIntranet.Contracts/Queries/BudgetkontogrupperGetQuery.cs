using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter grupper af budgetkonti.
    /// </summary>
    [DataContract(Name = "BudgetkontogrupperGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BudgetkontogrupperGetQuery : IQuery
    {
    }
}
