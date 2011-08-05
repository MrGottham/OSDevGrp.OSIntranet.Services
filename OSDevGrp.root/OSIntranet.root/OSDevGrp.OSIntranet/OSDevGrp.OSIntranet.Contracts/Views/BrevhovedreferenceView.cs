using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en reference til et brevhoved.
    /// </summary>
    [DataContract(Name = "Brevhovedreference", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BrevhovedreferenceView : TabelView
    {
    }
}
