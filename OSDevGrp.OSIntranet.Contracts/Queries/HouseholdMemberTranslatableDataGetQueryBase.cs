using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query for getting some translatable data for a household member.
    /// </summary>
    [DataContract(Name = "HouseholdMemberTranslatableDataGetQueryBase", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public abstract class HouseholdMemberTranslatableDataGetQueryBase : HouseholdMemberDataGetQueryBase
    {
        /// <summary>
        /// Gets or sets the identifier for the translation informations used for translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationInfoIdentifier { get; set; }
    }
}
