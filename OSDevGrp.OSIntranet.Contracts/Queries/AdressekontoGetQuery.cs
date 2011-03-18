using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en adressekonto.
    /// </summary>
    [DataContract(Name = "AdressekontoGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class AdressekontoGetQuery : AdressekontiGetQuery
    {
        /// <summary>
        /// Unik identifikation af adressekontoen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }
    }
}
