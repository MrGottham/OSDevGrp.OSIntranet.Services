using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en regnskabsliste.
    /// </summary>
    [DataContract(Name = "Regnskabsliste", Namespace = SoapNamespaces.IntranetNamespace)]
    public class RegnskabslisteView : TabelView
    {
        /// <summary>
        /// Brevhoved.
        /// </summary>
        [DataMember(IsRequired = false)]
        public BrevhovedreferenceView Brevhoved
        {
            get;
            set;
        }
    }
}
