using System.ServiceModel;

namespace OSDevGrp.OSIntranet.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til adressekartotek.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.AdressekartotekServiceName, Namespace = SoapNamespaces.IntranetNamespace)]
    public interface IAdressekartotekService : IIntranetService
    {
    }
}
