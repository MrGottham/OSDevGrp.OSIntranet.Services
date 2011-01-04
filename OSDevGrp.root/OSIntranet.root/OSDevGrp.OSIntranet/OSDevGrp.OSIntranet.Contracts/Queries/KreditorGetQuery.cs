using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en kreditor.
    /// </summary>
    [DataContract(Name = "KreditorGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KreditorGetQuery : KreditorlisteGetQuery
    {
        /// <summary>
        /// Unik identifikation af kreditoren.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }
    }
}
