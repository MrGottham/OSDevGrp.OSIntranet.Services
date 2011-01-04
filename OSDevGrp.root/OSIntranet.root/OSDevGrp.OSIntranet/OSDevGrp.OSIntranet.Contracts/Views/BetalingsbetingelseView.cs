using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobject for en betalingsbetingelse.
    /// </summary>
    [DataContract(Name = "Betalingsbetingelse", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BetalingsbetingelseView : TabelView
    {
    }
}
