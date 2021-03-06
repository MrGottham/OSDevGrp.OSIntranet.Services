﻿using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en budgetkontoplan.
    /// </summary>
    [DataContract(Name = "BudgetkontoplanGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BudgetkontoplanGetQuery : IQuery
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
        /// Statusdato.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime StatusDato
        {
            get;
            set;
        }
    }
}
