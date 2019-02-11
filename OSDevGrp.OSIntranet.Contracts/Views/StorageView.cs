using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// View for a storage.
    /// </summary>
    [DataContract(Name = "StorageView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class StorageView : StorageIdentificationView
    {
        /// <summary>
        /// Gets or sets the household on which the are placed.
        /// </summary>
        [DataMember(IsRequired = true)]
        public HouseholdIdentificationView Household { get; set; }

        /// <summary>
        /// Gets or sets the storage type.
        /// </summary>
        [DataMember(IsRequired = true)]
        public StorageTypeIdentificationView StorageType { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the temperature for the storage.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Temperature { get; set; }

        /// <summary>
        /// Gets or sets the creation time for the storage.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime CreationTime { get; set; }
    }
}
