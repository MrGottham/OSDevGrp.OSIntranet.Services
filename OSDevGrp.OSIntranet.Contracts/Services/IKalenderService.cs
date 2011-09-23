using System.ServiceModel;

namespace OSDevGrp.OSIntranet.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til kalender.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.KalenderServiceName, Namespace = SoapNamespaces.IntranetNamespace)]
    public interface IKalenderService : IIntranetService
    {
    }
}
