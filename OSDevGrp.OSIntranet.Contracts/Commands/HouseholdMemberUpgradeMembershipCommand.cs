using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for upgrading the membership on the current users household member account.
    /// </summary>
    [DataContract(Name = "HouseholdMemberUpgradeMembershipCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdMemberUpgradeMembershipCommand : HouseholdMemberDataModificationCommandBase
    {
        /// <summary>
        /// Gets or sets the membership which the household member should be upgraded to.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Membership { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the data provider who handled the payment for the upgrade.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid DataProviderIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the date and time for the payment for the upgrade.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime PaymentTime { get; set; }

        /// <summary>
        /// Gets or sets the payment reference from the data provider who handled the payment for the upgrade.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string PaymentReference { get; set; }

        /// <summary>
        /// Gets or sets the Base64 encoded payment receipt from the data provider who handled the payment for the upgrade.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string PaymentReceipt { get; set; }
    }
}
