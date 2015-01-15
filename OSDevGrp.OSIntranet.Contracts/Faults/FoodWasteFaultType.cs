using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Faults
{
    /// <summary>
    /// Type for a fault used in the food waste domain.
    /// </summary>
    [DataContract(Name = "FaultType", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public enum FoodWasteFaultType
    {
        [EnumMember]
        RepositoryFault,

        [EnumMember]
        SystemFault,

        [EnumMember]
        BusinessFault
    }
}
