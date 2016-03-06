using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for adding a household to the current users household account.
    /// </summary>
    [DataContract(Name = "HouseholdAddCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdAddCommand : ICommand
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

        /// <summary>
        /// Gets or sets the identifier of the translation informations which should be used in the translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationInfoIdentifier { get; set; }
    }
}
