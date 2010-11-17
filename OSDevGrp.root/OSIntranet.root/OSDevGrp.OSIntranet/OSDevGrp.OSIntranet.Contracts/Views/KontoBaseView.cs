using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Basisviewobject for en konto og budgetkonto.
    /// </summary>
    [DataContract(Name = "KontoBase", Namespace = SoapNamespaces.IntranetNamespace)]
    public abstract class KontoBaseView : IView
    {
        /// <summary>
        /// Regnskab.
        /// </summary>
        [DataMember(IsRequired = true)]
        public RegnskabslisteView Regnskab
        {
            get;
            set;
        }

        /// <summary>
        /// Kontonummer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonummer
        {
            get;
            set;
        }

        /// <summary>
        /// Kontonavn.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonavn
        {
            get;
            set;
        }

        /// <summary>
        /// Beskrivelse.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Beskrivelse
        {
            get;
            set;
        }

        /// <summary>
        /// Notat.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Notat
        {
            get;
            set;
        }
    }
}
