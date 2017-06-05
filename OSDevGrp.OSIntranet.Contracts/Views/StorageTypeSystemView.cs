using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// System view for a storage type.
    /// </summary>
    [DataContract(Name = "StorageTypeSystemView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class StorageTypeSystemView : StorageTypeIdentificationView
    {
        /// <summary>
        /// Gets or sets the order for sortering storage types.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the order for sortering storage types.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Temperature { get; set; }

        /// <summary>
        /// Gets or sets the temperature range for the storage type.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IntRangeView TemperatureRange { get; set; }

        /// <summary>
        /// Gets or sets whether household members can create storages of this type.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool Creatable { get; set; }

        /// <summary>
        /// Gets or sets whether household members can edit storages of this type.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool Editable { get; set; }

        /// <summary>
        /// Gets or sets whether household members can delete storages of this type.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool Deletable { get; set; }

        /// <summary>
        /// Gets or sets the translations for the storage type.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<TranslationSystemView> Translations { get; set; }
    }
}
