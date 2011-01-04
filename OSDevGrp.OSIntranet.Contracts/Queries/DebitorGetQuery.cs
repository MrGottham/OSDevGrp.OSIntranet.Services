using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en debitor.
    /// </summary>
    [DataContract(Name = "DebitorGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class DebitorGetQuery : DebitorlisteGetQuery
    {
        /// <summary>
        /// Unik identifikation af debitoren.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }
    }
}
