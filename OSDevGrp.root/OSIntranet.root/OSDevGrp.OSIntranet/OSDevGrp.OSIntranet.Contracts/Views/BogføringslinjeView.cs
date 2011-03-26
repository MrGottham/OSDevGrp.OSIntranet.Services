using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en bogføringslinje.
    /// </summary>
    [DataContract(Name = "Bogføringslinje", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BogføringslinjeView : IView
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
    }
}
