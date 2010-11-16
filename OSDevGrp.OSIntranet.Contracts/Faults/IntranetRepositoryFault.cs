using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Faults
{
    /// <summary>
    /// Fault for en fejl i repositories under OS Intranet.
    /// </summary>
    [DataContract(Name = "IntranetRepositoryFault", Namespace = SoapNamespaces.IntranetNamespace)]
    public class IntranetRepositoryFault : IntranetFaultBase
    {
    }
}
