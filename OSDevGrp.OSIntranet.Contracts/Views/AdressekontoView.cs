using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en adressekonto.
    /// </summary>
    [DataContract(Name = "Adressekonto", Namespace = SoapNamespaces.IntranetNamespace)]
    public class AdressekontoView : SaldoBaseView
    {
    }
}
