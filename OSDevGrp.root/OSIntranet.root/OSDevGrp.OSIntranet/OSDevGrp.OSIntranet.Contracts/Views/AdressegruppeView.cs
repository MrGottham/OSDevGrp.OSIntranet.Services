using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobject for en adressegruppe.
    /// </summary>
    [DataContract(Name = "Adressegruppe", Namespace = SoapNamespaces.IntranetNamespace)]
    public class AdressegruppeView : TabelView
    {
        /// <summary>
        /// Unik identifikation af den tilsvarende adressegruppe i OSWEBDB.
        /// </summary>
        [DataMember(IsRequired = false)]
        public int AdressegruppeOswebdb
        {
            get;
            set;
        }
    }
}
