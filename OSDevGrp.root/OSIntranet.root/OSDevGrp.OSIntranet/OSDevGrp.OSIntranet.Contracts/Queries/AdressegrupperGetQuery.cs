using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter adressegrupper.
    /// </summary>
    [DataContract(Name = "AdressegrupperGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class AdressegrupperGetQuery : IQuery
    {
    }
}
