using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Faults
{
    /// <summary>
    /// Basisklasse for en OS Intranet Service Fault.
    /// </summary>
    [DataContract(Name = "IntranetFault", Namespace = SoapNamespaces.IntranetNamespace)]
    public abstract class IntranetFaultBase
    {
        /// <summary>
        /// Fejlbesked.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Fejlbeskeder fra exception og inner exceptions.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string ExceptionMessages
        {
            get;
            set;
        }
    }
}
