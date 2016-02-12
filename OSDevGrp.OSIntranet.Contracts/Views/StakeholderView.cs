using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// View for an internal or external stakeholder.
    /// </summary>
    [DataContract(Name = "StakeholderView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class StakeholderView : IView
    {
        /// <summary>
        /// Gets or sets the identfier for the internal or external stakeholder.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid StakeholderIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the type of the internal or external stakeholder.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string StakeholderType { get; set; }

        /// <summary>
        /// Gets or sets the mail address for the  internal or external stakeholder.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string MailAddress { get; set; }
    }
}
