using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Identification view for a household member.
    /// </summary>
    [DataContract(Name = "HouseholdMemberIdentificationView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdMemberIdentificationView : IView
    {
        /// <summary>
        /// Gets or sets the identifier for the household member.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid HouseholdMemberIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the mail address for the household member.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string MailAddress { get; set; }
    }
}
