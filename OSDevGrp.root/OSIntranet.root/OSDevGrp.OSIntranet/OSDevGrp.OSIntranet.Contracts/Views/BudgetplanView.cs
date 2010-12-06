using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobject for en budgetplan.
    /// </summary>
    [DataContract(Name = "Budgetplan", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BudgetplanView : BudgetkontogruppeView
    {
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

        /// <summary>
        /// Budgetkonti.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<BudgetkontoplanView> Budgetkonti
        {
            get;
            set;
        }
    }
}
