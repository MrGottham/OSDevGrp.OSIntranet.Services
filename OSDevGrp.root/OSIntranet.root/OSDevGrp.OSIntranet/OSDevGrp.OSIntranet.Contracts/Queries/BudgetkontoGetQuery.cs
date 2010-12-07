using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en budgetkonto.
    /// </summary>
    [DataContract(Name = "BudgetkontoGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BudgetkontoGetQuery : BudgetkontoplanGetQuery
    {
        /// <summary>
        /// Budgetkontonummer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonummer
        {
            get;
            set;
        }
    }
}
