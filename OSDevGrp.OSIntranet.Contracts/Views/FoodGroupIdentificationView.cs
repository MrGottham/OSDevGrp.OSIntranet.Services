using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Identification view for a food group.
    /// </summary>
    [DataContract(Name = "FoodGroupIdentificationView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodGroupIdentificationView : IView
    {
        /// <summary>
        /// Gets or sets the identification for the food group.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid FoodGroupIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the name for the food group.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}
