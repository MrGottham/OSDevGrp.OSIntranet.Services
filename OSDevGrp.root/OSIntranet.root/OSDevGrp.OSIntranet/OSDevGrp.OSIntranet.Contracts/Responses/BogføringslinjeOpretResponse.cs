using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Contracts.Responses
{
    /// <summary>
    /// Response fra oprettelse af en bogføringslinje.
    /// </summary>
    [DataContract(Name = "BogføringslinjeOpretResp", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BogføringslinjeOpretResponse : IView
    {
        /// <summary>
        /// Unik identifikation af bogføringslinjen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Løbenr
        {
            get;
            set;
        }

        /// <summary>
        /// Konto.
        /// </summary>
        [DataMember(IsRequired = true)]
        public KontoplanView Konto
        {
            get;
            set;
        }

        /// <summary>
        /// Budgetkonto.
        /// </summary>
        [DataMember(IsRequired = false)]
        public BudgetkontoplanView Budgetkonto
        {
            get;
            set;
        }

        /// <summary>
        /// Adressekonto.
        /// </summary>
        [DataMember(IsRequired = false)]
        public AdressekontolisteView Adressekonto
        {
            get;
            set;
        }

        /// <summary>
        /// Bogføringsdato.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime Dato
        {
            get;
            set;
        }

        /// <summary>
        /// Bilagsnummer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Bilag
        {
            get;
            set;
        }

        /// <summary>
        /// Tekst.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Tekst
        {
            get;
            set;
        }

        /// <summary>
        /// Debit.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Debit
        {
            get;
            set;
        }

        /// <summary>
        /// Kredit.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Kredit
        {
            get;
            set;
        }

        /// <summary>
        /// Advarsler i forbindelse med oprettelse af bogføringslinjer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<BogføringsadvarselResponse> Advarsler
        {
            get;
            set;
        }
    }
}
