using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter et antal bogføringslinjer.
    /// </summary>
    [DataContract(Name = "BogføringerGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BogføringerGetQuery : IQuery
    {
        /// <summary>
        /// Regnskabsnummer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Regnskabsnummer
        {
            get;
            set;
        }

        /// <summary>
        /// Statusdato.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime StatusDato
        {
            get;
            set;
        }

        /// <summary>
        /// Antal bogføringslinjer, der skal hentes.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Linjer
        {
            get;
            set;
        }
    }
}
