using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en basisadresse.
    /// </summary>
    [DataContract(Name = "AdresseBase", Namespace = SoapNamespaces.IntranetNamespace)]
    public abstract class AdresseBaseView : TelefonlisteView
    {
        /// <summary>
        /// Adresse (linje 1).
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Adresse1
        {
            get;
            set;
        }

        /// <summary>
        /// Adresse (linje 2).
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Adresse2
        {
            get;
            set;
        }

        /// <summary>
        /// Postnummer og bynavn.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string PostnummerBy
        {
            get;
            set;
        }

        /// <summary>
        /// Adressegruppe.
        /// </summary>
        [DataMember(IsRequired = true)]
        public AdressegruppeView Adressegruppe
        {
            get;
            set;
        }

        /// <summary>
        /// Bekendtskab.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Bekendtskab
        {
            get;
            set;
        }

        /// <summary>
        /// Mailadresse.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Mailadresse
        {
            get;
            set;
        }

        /// <summary>
        /// Hjemmeside.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Web
        {
            get;
            set;
        }

        /// <summary>
        /// Betalingsbetingelse.
        /// </summary>
        [DataMember(IsRequired = true)]
        public BetalingsbetingelseView Betalingsbetingelse
        {
            get;
            set;
        }

        /// <summary>
        /// Udlånsfrist.
        /// </summary>
        [DataMember(IsRequired = false)]
        public int Udlånsfrist
        {
            get;
            set;
        }
    }
}
