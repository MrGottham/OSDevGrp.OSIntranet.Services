using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command til oprettelse af en bogføringslinje.
    /// </summary>
    [DataContract(Name = "BogføringslinjeOpretCommand", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BogføringslinjeOpretCommand : ICommand
    {
        /// <summary>
        /// Regnskabsnummer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Regnskabsnummer
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
        /// Kontonummer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonummer
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
        /// Budgetkontonummer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Budgetkontonummer
        {
            get;
            set;
        }

        /// <summary>
        /// Debitbeløb.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Debit
        {
            get;
            set;
        }

        /// <summary>
        /// Kreditbeløb.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Kredit
        {
            get;
            set;
        }
    }
}
