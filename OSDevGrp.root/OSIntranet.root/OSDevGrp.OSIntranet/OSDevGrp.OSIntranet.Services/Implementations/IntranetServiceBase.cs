using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts;

namespace OSDevGrp.OSIntranet.Services.Implementations
{
    /// <summary>
    /// Basisklasse for en service til OS Intranet.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, Namespace = SoapNamespaces.IntranetNamespace)]
    public abstract class IntranetServiceBase
    {
    }
}