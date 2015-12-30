using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for adding a household member.
    /// </summary>
    [DataContract(Name = "HouseholdMemberAddCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdMemberAddCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the mail address for the household member.
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
