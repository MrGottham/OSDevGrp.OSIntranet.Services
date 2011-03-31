using System.Collections.Generic;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Responses
{
    /// <summary>
    /// Response fra oprettelse af en bogføringslinje.
    /// </summary>
    [DataContract(Name = "BogføringslinjeOpretResponse", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BogføringslinjeOpretResponse : IView
    {
        /// <summary>
        /// Advarsler i forbindelse med oprettelse af bogføringslinjer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<string> Advarsler
        {
            get;
            set;
        }
    }
}
