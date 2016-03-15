using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for modifying some data on a given household on the current household member.
    /// </summary>
    [DataContract(Name = "HouseholdDataModificationCommandBase", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public abstract class HouseholdDataModificationCommandBase : HouseholdMemberDataModificationCommandBase
    {
        /// <summary>
        /// Gets or sets the identifier for the household on which to modify data.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid HouseholdIdentifier { get; set; }
    }
}
