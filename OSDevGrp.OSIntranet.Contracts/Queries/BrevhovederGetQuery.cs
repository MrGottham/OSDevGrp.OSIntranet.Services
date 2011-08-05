using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter brevhoveder.
    /// </summary>
    [DataContract(Name = "BrevhoverGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BrevhovederGetQuery : IQuery
    {
    }
}
