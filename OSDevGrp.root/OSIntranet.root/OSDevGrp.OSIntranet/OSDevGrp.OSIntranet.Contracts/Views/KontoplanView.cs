using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobject for en kontoplan.
    /// </summary>
    [DataContract(Name = "Kontoplan", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KontoplanView : KontoBaseView
    {
        /// <summary>
        /// Kontogruppe.
        /// </summary>
        [DataMember(IsRequired = true)]
        public KontogruppeView Kontogruppe
        {
            get;
            set;
        }

        /// <summary>
        /// Kredit.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Kredit
        {
            get;
            set;
        }

        /// <summary>
        /// Saldo.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Saldo
        {
            get;
            set;
        }

        /// <summary>
        /// Desponibel beløb.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Disponibel
        {
            get;
            set;
        }
    }
}
