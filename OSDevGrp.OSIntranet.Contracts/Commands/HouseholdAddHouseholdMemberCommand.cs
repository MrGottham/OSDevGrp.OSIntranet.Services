using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for adding a household member to a given household on the current users household account.
    /// </summary>
    [DataContract(Name = "HouseholdAddHouseholdMemberCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdAddHouseholdMemberCommand : HouseholdDataModificationCommandBase
    {
        /// <summary>
        /// Gets or sets the mail address for the household member to add.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string MailAddress { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the translation informations which should be used in the translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationInfoIdentifier { get; set; }
    }
}
