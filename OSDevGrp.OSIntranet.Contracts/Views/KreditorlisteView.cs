using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en kreditorliste.
    /// </summary>
    [DataContract(Name = "Kreditorliste", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KreditorlisteView : TelefonlisteView
    {
        /// <summary>
        /// Saldo.
        /// </summary>
        [DataMember(IsRequired = true)]
        public decimal Saldo
        {
            get;
            set;
        }
    }
}
