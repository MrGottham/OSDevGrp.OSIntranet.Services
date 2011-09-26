using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en telefonliste.
    /// </summary>
    [DataContract(Name = "TelefonlisteGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class TelefonlisteGetQuery : IQuery
    {
    }
}
