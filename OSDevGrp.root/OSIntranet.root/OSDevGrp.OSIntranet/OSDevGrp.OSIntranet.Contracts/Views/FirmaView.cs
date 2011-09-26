using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for et firma.
    /// </summary>
    [DataContract(Name = "Firma", Namespace = SoapNamespaces.IntranetNamespace)]
    public class FirmaView : AdresseBaseView
    {
        /// <summary>
        /// Telefax.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Telefax
        {
            get;
            set;
        }

        /// <summary>
        /// Tilknyttede personer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public IEnumerable<TelefonlisteView> Personer
        {
            get;
            set;
        }
    }
}
