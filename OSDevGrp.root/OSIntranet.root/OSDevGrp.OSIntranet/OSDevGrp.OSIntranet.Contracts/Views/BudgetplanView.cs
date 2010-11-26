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
