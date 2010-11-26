using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobject for en kontogruppe.
    /// </summary>
    [DataContract(Name = "Kontogruppe", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KontogruppeView : TabelView
    {
        /// <summary>
        /// Angivelse af, om kontogruppen er aktiver.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool ErAktiver
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af, om kontogruppen er passiver.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool ErPassiver
        {
            get;
            set;
        }
    }
}
