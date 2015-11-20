using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// View for a food item.
    /// </summary>
    [DataContract(Name = "FoodItemView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodItemView : FoodItemIdentificationView
    {
        /// <summary>
        /// Gets or sets the primary food group.
        /// </summary>
        [DataMember(IsRequired = true)]
        public FoodGroupIdentificationView PrimaryFoodGroup { get; set; }

        /// <summary>
        /// Gets or sets whether the food item is active.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the food groups which this food item belong to.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<FoodGroupIdentificationView> FoodGroups { get; set; }
    }
}
