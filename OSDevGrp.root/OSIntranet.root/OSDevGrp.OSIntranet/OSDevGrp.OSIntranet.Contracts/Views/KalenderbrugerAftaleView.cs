using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en kalenderaftale til en given kalenderbruger.
    /// </summary>
    [DataContract(Name = "KalenderbrugerAftale", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KalenderbrugerAftaleView : IView
    {
        /// <summary>
        /// System under OSWEBDB, som kalenderaftalen er tilknyttet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public SystemView System
        {
            get;
            set;
        }

        /// <summary>
        /// Unik identifikation af kalenderaftalen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Tidspunkt, hvorfra kalenderaftalen er gældende.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime FraTidspunkt
        {
            get;
            set;
        }

        /// <summary>
        /// Tidspunkt, hvortil kalenderaftalen er gældende.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime TilTidspunkt
        {
            get;
            set;
        }

        /// <summary>
        /// Emne for kalenderaftalen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Emne
        {
            get;
            set;
        }

        /// <summary>
        /// Notat for kalenderaftalen.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Notat
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af, om kalenderaftalen er offentlig.
        /// </summary>
        [DataMember(IsRequired = false)]
        public bool Offentlig
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af, om kalenderaftalen er privat.
        /// </summary>
        [DataMember(IsRequired = false)]
        public bool Privat
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af, om der er alarm på det tidspunkt, hvorfra kalenderaftalen er gældende.
        /// </summary>
        [DataMember(IsRequired = false)]
        public bool Alarm
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af, om kalenderaftalen er udført.
        /// </summary>
        [DataMember(IsRequired = false)]
        public bool Udført
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af, om kalenderaftalen skal eksporteres.
        /// </summary>
        [DataMember(IsRequired = false)]
        public bool Eksporteres
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af, om kalenderaftalen er eksporteret.
        /// </summary>
        [DataMember(IsRequired = false)]
        public bool Eksporteret
        {
            get;
            set;
        }

        /// <summary>
        /// Deltagere til kalenderaftalen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<KalenderbrugerView> Deltagere
        {
            get;
            set;
        }
    }
}
