using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// View for a household member.
    /// </summary>
    [DataContract(Name = "HouseholdMemberView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdMemberView : HouseholdMemberIdentificationView
    {
        /// <summary>
        /// Gets or sets the membership.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Membership { get; set; }

        /// <summary>
        /// Gets or sets the date and time for when the membership expires.
        /// </summary>
        [DataMember(IsRequired = false)]
        public DateTime? MembershipExpireTime { get; set; }

        /// <summary>
        /// Gets or sets the identification of whether the membership can be renewed.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool CanRenewMembership { get; set; }

        /// <summary>
        /// Gets or sets the identification ofwhether the membership can be upgraded.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool CanUpgradeMembership { get; set; }

        /// <summary>
        /// Gets or sets the date and time for when the household member was activated.
        /// </summary>
        [DataMember(IsRequired = false)]
        public DateTime? ActivationTime { get; set; }

        /// <summary>
        /// Gets or sets the identification of whether the household member is activated or not.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool IsActivated { get; set; }

        /// <summary>
        /// Gets or sets the date and time for when the household member has accepted our privacy policy.
        /// </summary>
        [DataMember(IsRequired = false)]
        public DateTime? PrivacyPolicyAcceptedTime { get; set; }

        /// <summary>
        /// Get or sets the identification of whether the household member has accepted our privacy policy or not.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool IsPrivacyPolictyAccepted { get; set; }

        /// <summary>
        /// Gets or sets the identification of whether the household member has reached the household limit.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool HasReachedHouseholdLimit { get; set; }

        /// <summary>
        /// Gets or sets the date and time for when the household member was created.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the memberships which the household member can upgrade to.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<string> UpgradeableMemberships { get; set; }

        /// <summary>
        /// Gets or sets the households on which the household member has a membership.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<HouseholdIdentificationView> Households { get; set; }

        /// <summary>
        /// Gets or sets the payments made by the household member.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<PaymentView> Payments { get; set; }
    }
}
