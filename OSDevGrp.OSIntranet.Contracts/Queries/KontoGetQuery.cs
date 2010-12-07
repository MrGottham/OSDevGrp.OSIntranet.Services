using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en konto.
    /// </summary>
    [DataContract(Name = "KontoGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KontoGetQuery : KontoplanGetQuery
    {
        /// <summary>
        /// Kontonummer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonummer
        {
            get;
            set;
        }
    }
}
