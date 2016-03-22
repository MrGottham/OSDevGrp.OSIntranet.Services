using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for removing a household member from a given household on the current users household account.
    /// </summary>
    [DataContract(Name = "HouseholdRemoveHouseholdMemberCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdRemoveHouseholdMemberCommand : HouseholdDataModificationCommandBase
    {
        /// <summary>
        /// Gets or sets the mail address for the household member to remove.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string MailAddress { get; set; }
    }
}
