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
        /// Bogført beløb.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Bogført
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
