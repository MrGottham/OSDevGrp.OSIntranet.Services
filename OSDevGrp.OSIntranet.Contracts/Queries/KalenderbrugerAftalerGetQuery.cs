using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter kalenderaftaler til en given kalenderbrugere.
    /// </summary>
    [DataContract(Name = "KalenderbrugerAftalerGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KalenderbrugerAftalerGetQuery : IQuery
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
        /// Initialer på den kalenderbruger, som der skal hentes kalenderaftaler til.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Initialer
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af den dato, hvorfra kalenderaftaler skal hentes.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime FraDato
        {
            get;
            set;
        }
    }
}
