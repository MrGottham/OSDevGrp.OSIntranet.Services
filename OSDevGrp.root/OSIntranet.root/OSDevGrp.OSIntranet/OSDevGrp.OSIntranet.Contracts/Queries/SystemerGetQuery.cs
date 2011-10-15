using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter systemer under OSWEBDB.
    /// </summary>
    [DataContract(Name = "SystemerGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class SystemerGetQuery : IQuery
    {
    }
}
