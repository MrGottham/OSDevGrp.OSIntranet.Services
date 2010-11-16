using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Faults
{
    /// <summary>
    /// Fault for en fejl i forretningslogik under OS Intranet.
    /// </summary>
    [DataContract(Name = "IntranetBusinessFault", Namespace = SoapNamespaces.IntranetNamespace)]
    public class IntranetBusinessFault : IntranetFaultBase
    {
    }
}
