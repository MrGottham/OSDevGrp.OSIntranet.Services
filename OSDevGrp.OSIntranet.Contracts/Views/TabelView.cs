﻿using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for generelle tabeloplysninger.
    /// </summary>
    [DataContract(Name = "Tabel", Namespace = SoapNamespaces.IntranetNamespace)]
    public abstract class TabelView : IView
    {
        /// <summary>
        /// Nummer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }
    }
}
