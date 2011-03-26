using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter kontogrupper.
    /// </summary>
    [DataContract(Name = "KontogrupperGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KontogrupperGetQuery : IQuery
    {
    }
}
