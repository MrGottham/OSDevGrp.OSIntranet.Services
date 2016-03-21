using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for updatering a household to the current users household account.
    /// </summary>
    [DataContract(Name = "HouseholdUpdateCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdUpdateCommand : HouseholdDataModificationCommandBase
    {
        /// <summary>
        /// Gets or sets the name for the household.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description for the household.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Description { get; set; }
    }
}
