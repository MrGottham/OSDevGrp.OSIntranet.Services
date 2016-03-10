using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// View for a household.
    /// </summary>
    [DataContract(Name = "HouseholdView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdView : HouseholdIdentificationView
    {
        /// <summary>
        /// Gets or sets the description for the household.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the date and time for when the household was created.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the household members who is member of this household.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<HouseholdMemberIdentificationView> HouseholdMembers { get; set; }
    }
}
