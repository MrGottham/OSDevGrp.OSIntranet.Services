using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobject for en gruppe af budgetkonti.
    /// </summary>
    [DataContract(Name = "Budgetkontogruppe", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BudgetkontogruppeView : TabelView
    {
    }
}
