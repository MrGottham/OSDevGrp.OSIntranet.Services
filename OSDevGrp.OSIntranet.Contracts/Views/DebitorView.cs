using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en debitor.
    /// </summary>
    [DataContract(Name = "Debitor", Namespace = SoapNamespaces.IntranetNamespace)]
    public class DebitorView : SaldoBaseView
    {
    }
}
