using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// System view for a food group.
    /// </summary>
    [DataContract(Name = "FoodGroupSystemView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodGroupSystemView : FoodGroupIdentificationView
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
        /// Gets or sets the translations for the food group.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<TranslationSystemView> Translations { get; set; }

        /// <summary>
        /// Gets or sets the foreign keys for the food group.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<ForeignKeySystemView> ForeignKeys { get; set; }

        /// <summary>
        /// Gets or sets the foods groups which has this food group as a parent. 
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<FoodGroupSystemView> Children { get; set; }
    }
}
