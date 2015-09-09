using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// View for a food group.
    /// </summary>
    [DataContract(Name = "FoodGroupView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodGroupView : FoodGroupIdentificationView
    {
        /// <summary>
        /// Gets or sets whether the food group is active.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the food group which has this food group as a child.
        /// </summary>
        [DataMember(IsRequired = false)]
        public FoodGroupIdentificationView Parent { get; set; }

        /// <summary>
        /// Gets or sets the foods groups which has this food group as a parent. 
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<FoodGroupView> Children { get; set; }
    }
}
