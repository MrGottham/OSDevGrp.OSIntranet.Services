using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en debitorliste.
    /// </summary>
    [DataContract(Name = "Debitorliste", Namespace = SoapNamespaces.IntranetNamespace)]
    public class DebitorlisteView : SaldolisteBaseView
    {
    }
}
