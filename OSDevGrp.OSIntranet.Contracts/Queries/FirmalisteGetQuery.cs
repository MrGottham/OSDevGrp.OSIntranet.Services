using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en liste af firmaer.
    /// </summary>
    [DataContract(Name = "FirmalisteGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class FirmalisteGetQuery : IQuery
    {
    }
}
