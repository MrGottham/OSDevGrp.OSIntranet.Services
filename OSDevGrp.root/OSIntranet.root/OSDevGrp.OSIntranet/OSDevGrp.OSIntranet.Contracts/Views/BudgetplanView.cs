using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobject for en budgetkontoplan.
    /// </summary>
    [DataContract(Name = "Budgetkontoplan", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BudgetplanView : BudgetkontogruppeView
    {
    }
}
