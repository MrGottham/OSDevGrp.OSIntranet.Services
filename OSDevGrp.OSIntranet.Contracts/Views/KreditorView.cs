using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en kreditor.
    /// </summary>
    [DataContract(Name = "Kreditor", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KreditorView : SaldoBaseView
    {
    }
}
