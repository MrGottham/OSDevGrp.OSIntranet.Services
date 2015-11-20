using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// System view for a food item.
    /// </summary>
    [DataContract(Name = "FoodItemSystemView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodItemSystemView : FoodItemIdentificationView
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
        public IEnumerable<FoodGroupSystemView> FoodGroups { get; set; }

        /// <summary>
        /// Gets or sets the translations for the food group.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<TranslationSystemView> Translations { get; set; }

        /// <summary>
        /// Gets or sets the foreign keys for the food group.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<ForeignKeySystemView> ForeignKeys { get; set; }
    }
}
