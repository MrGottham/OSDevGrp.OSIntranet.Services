using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en liste af adressekonti.
    /// </summary>
    [DataContract(Name = "Adressekontoliste", Namespace = SoapNamespaces.IntranetNamespace)]
    public class AdressekontolisteView : SaldolisteBaseView
    {
    }
}
