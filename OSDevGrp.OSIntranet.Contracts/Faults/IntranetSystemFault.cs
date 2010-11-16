using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Faults
{
    /// <summary>
    /// Fault for en systemfejl i OS Intranet.
    /// </summary>
    [DataContract(Name = "IntranetSystemFault", Namespace = SoapNamespaces.IntranetNamespace)]
    public class IntranetSystemFault : IntranetFaultBase
    {
    }
}
