using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Faults
{
    /// <summary>
    /// Fault which can be used in the food waste domain.
    /// </summary>
    [DataContract(Name = "Fault", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodWasteFault
    {
        /// <summary>
        /// Type of fault.
        /// </summary>
        [DataMember(IsRequired = true)]
        public FoodWasteFaultType FaultType { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Name of the service which fails.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string ServiceName { get; set; }

        /// <summary>
        /// Name of the service method which fails.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string ServiceMethod { get; set; }

        /// <summary>
        /// Stack trance for the fault.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string StackTrace { get; set; }
    }
}
