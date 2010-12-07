using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobject for budgetoplysninger.
    /// </summary>
    [DataContract(Name = "Budgetoplysninger", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BudgetoplysningerView : MånedsoplysningerView
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
    }
}
