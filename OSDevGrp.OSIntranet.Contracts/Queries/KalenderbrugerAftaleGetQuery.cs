using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en given kalenderaftale til en given kalenderbruger.
    /// </summary>
    [DataContract(Name = "KalenderbrugerAftaleGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KalenderbrugerAftaleGetQuery : IQuery
    {
        /// <summary>
        /// Unik identifikation af systemet under OSWEBDB, hvorfra kalenderaftaler skal hentes.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int System
        {
            get;
            set;
        }

        /// <summary>
        /// Initialer på den kalenderbruger, som kalenderaftalen skal hentes til.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Initialer
        {
            get;
            set;
        }

        /// <summary>
        /// Unik identifikation af kalenderaftalen, som skal hentes for kalenderbrugeren.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int AftaleId
        {
            get;
            set;
        }
    }
}
