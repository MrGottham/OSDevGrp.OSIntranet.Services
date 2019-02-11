using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Identification view for a storage.
    /// </summary>
    [DataContract(Name = "StorageIdentificationView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class StorageIdentificationView : IView
    {
        /// <summary>
        /// Gets or sets the identifier for the storage.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid StorageIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the sort order for the storage.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int SortOrder { get; set; }
    }
}
