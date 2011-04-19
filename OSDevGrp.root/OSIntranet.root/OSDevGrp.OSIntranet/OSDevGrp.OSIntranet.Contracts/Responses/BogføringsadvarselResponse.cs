using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Contracts.Responses
{
    /// <summary>
    /// Response for en bogføringsadvarsel.
    /// </summary>
    [DataContract(Name = "BogføringsadvarselResp", Namespace = SoapNamespaces.IntranetNamespace)]
    [KnownType(typeof(KontoView))]
    [KnownType(typeof(BudgetkontoView))]
    public class BogføringsadvarselResponse : IView
    {
        /// <summary>
        /// Tekst for advarsel.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Advarsel
        {
            get;
            set;
        }

        /// <summary>
        /// Konto, hvorpå advarslen er opstået.
        /// </summary>
        [DataMember(IsRequired = true)]
        public KontoBaseView Konto
        {
            get;
            set;
        }

        /// <summary>
        /// Beløb for advarslen, eksempelvis beløbet, som kontoen er overtrukket med.
        /// </summary>
        [DataMember(IsRequired = true)]
        public decimal Beløb
        {
            get;
            set;
        }
    }
}
