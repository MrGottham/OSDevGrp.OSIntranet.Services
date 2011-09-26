using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en person.
    /// </summary>
    [DataContract(Name = "Person", Namespace = SoapNamespaces.IntranetNamespace)]
    public class PersonView : AdresseBaseView
    {
        /// <summary>
        /// Fødselsdato.
        /// </summary>
        [DataMember(IsRequired = false)]
        public DateTime? Fødselsdato
        {
            get;
            set;
        }

        /// <summary>
        /// Firma.
        /// </summary>
        [DataMember(IsRequired = false)]
        public TelefonlisteView Firma
        {
            get;
            set;
        }
    }
}
