using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobject for en budgetkontoplan.
    /// </summary>
    [DataContract(Name = "Budgetkontoplan", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BudgetkontoplanView : KontoBaseView
    {
        /// <summary>
        /// Kontogruppe.
        /// </summary>
        [DataMember(IsRequired = true)]
        public BudgetkontogruppeView Budgetkontogruppe
        {
            get;
            set;
        }

        /// <summary>
        /// Budget.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Budget
        {
            get;
            set;
        }

        /// <summary>
        /// Budget sidste måned.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal BudgetSidsteMåned
        {
            get;
            set;
        }

        /// <summary>
        /// Budget år til dato.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal BudgetÅrTilDato
        {
            get;
            set;
        }

        /// <summary>
        /// Budget sidste år.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal BudgetSidsteÅr
        {
            get;
            set;
        }

        /// <summary>
        /// Bogført beløb.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Bogført
        {
            get;
            set;
        }

        /// <summary>
        /// Bogført beløb sidste måned.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal BogførtSidsteMåned
        {
            get;
            set;
        }

        /// <summary>
        /// Bogført beløb år til dato.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal BogførtÅrTilDato
        {
            get;
            set;
        }

        /// <summary>
        /// Bogført beløb sidste år.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal BogførtSidsteÅr
        {
            get;
            set;
        }

        /// <summary>
        /// Disponibel beløb.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Disponibel
        {
            get;
            set;
        }
    }
}
