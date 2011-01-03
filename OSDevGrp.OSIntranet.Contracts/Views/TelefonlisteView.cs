using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en telefonliste.
    /// </summary>
    [DataContract(Name = "Telefonliste", Namespace = SoapNamespaces.IntranetNamespace)]
    public class TelefonlisteView : IView
    {
        /// <summary>
        /// Unik identifikation af adresseposten.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn på adressen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }

        /// <summary>
        /// Primær telefonnummer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string PrimærTelefon
        {
            get;
            set;
        }

        /// <summary>
        /// Sekundær telefonummer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string SekundærTelefon
        {
            get;
            set;
        }
    }
}
