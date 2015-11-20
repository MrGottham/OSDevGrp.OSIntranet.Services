using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Identification view for a food item.
    /// </summary>
    [DataContract(Name = "FoodItemIdentificationView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodItemIdentificationView : IView
    {
        /// <summary>
        /// Gets or sets the identification for the food item.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid FoodItemIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the name for the food item.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}
