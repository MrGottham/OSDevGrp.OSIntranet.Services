using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en liste af personer.
    /// </summary>
    [DataContract(Name = "PersonlisteGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class PersonlisteGetQuery : IQuery
    {
    }
}
