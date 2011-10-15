using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter kalenderbrugere til et givent system under OSWEBDB.
    /// </summary>
    [DataContract(Name = "KalenderbrugereGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KalenderbrugereGetQuery : IQuery
    {
        /// <summary>
        /// Unik identifikation af systemet under OSWEBDB, hvorfra kalenderbrugere skal hentes.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int System
        {
            get;
            set;
        }
    }
}
