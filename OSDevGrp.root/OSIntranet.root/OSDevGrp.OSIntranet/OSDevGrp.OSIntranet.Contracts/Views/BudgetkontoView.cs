using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobject for en budgetkonto.
    /// </summary>
    [DataContract(Name = "Budgetkonto", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BudgetkontoView : BudgetkontoplanView
    {
        /// <summary>
        /// Budgetoplysninger.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<BudgetoplysningerView> Budgetoplysninger
        {
            get;
            set;
        }
    }
}
