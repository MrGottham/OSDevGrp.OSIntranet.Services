using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Basisviewobjekt for en saldo til henholdsvis debitorer og kreditorer.
    /// </summary>
    [DataContract(Name = "SaldoBase", Namespace = SoapNamespaces.IntranetNamespace)]
    public abstract class SaldoBaseView : SaldolisteBaseView
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
        /// Mailadresse.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Mailadresse
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
    }
}
