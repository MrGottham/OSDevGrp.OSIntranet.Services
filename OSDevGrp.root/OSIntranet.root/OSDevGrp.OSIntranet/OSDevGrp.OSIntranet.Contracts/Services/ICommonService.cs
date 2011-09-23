using System.ServiceModel;

namespace OSDevGrp.OSIntranet.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til fælles elementer.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.CommonServiceName, Namespace = SoapNamespaces.IntranetNamespace)]
    public interface ICommonService : IIntranetService
    {
    }
}
