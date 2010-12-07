using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobject for en konto.
    /// </summary>
    [DataContract(Name = "Konto", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KontoView : KontoplanView
    {
        /// <summary>
        /// Kreditoplysninger.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<KreditoplysningerView> Kreditoplysninger
        {
            get;
            set;
        }
    }
}
