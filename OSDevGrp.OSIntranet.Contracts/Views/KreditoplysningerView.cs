using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobject for kreditoplysninger.
    /// </summary>
    [DataContract(Name = "Kreditoplysninger", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KreditoplysningerView : MånedsoplysningerView
    {
        /// <summary>
        /// Kredit.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Kredit
        {
            get;
            set;
        }
    }
}
