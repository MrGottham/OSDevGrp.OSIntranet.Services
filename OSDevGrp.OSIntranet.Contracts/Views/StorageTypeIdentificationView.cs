using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Identification view for a storage type.
    /// </summary>
    [DataContract(Name = "StorageTypeIdentificationView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class StorageTypeIdentificationView : IView
    {
        /// <summary>
        /// Gets or sets the identification for the storage type.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid StorageTypeIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the name for the storage type.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}
