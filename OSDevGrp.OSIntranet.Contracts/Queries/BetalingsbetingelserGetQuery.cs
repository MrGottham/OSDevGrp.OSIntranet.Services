using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter betalingsbetingelser.
    /// </summary>
    [DataContract(Name = "BetalingsbetingelserGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BetalingsbetingelserGetQuery : IQuery
    {
    }
}
