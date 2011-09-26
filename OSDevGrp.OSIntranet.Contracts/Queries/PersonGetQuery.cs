using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en given person.
    /// </summary>
    [DataContract(Name = "PersonGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class PersonGetQuery : IQuery
    {
        /// <summary>
        /// Unik identifikaton af personen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }
    }
}
