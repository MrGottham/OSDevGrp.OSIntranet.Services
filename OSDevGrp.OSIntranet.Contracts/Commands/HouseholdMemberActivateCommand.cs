using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for activating the current users household member account.
    /// </summary>
    [DataContract(Name = "HouseholdMemberActivateCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdMemberActivateCommand : HouseholdMemberDataModificationCommandBase
    {
        /// <summary>
        /// Gets or sets the activation code used to activate the household member.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string ActivationCode { get; set; }
    }
}
