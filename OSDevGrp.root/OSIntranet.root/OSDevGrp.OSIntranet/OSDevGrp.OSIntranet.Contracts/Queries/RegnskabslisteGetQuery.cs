using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en regnskabsliste.
    /// </summary>
    [DataContract(Name = "RegnskabslisteGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class RegnskabslisteGetQuery : IQuery
    {
    }
}
