using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter postnumre.
    /// </summary>
    [DataContract(Name = "PostnumreGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class PostnumreGetQuery : IQuery
    {
    }
}
