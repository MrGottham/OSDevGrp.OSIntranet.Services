using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Identification view for a household.
    /// </summary>
    [DataContract(Name = "HouseholdIdentificationView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdIdentificationView : IView
    {
        /// <summary>
        /// Gets or sets the identifier for the household.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid HouseholdIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the description for the household.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Description { get; set; }
    }
}
