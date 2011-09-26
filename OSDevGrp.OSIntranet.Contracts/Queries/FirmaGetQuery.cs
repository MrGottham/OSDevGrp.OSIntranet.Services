using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter et givent firma.
    /// </summary>
    [DataContract(Name = "FirmaGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class FirmaGetQuery : IQuery
    {
        /// <summary>
        /// Unik identifikaton af firmaet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }
    }
}
