namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a storage.
    /// </summary>
    public interface IStorage : IIdentifiable
    {
        /// <summary>
        /// Gets the household where the storage are placed.
        /// </summary>
        IHousehold Household { get; }

        /// <summary>
        /// Gets or sets the sort order for the storage.
        /// </summary>
        int SortOrder { get; set; }

        /// <summary>
        /// Gets the storage type for the storage.
        /// </summary>
        IStorageType StorageType { get; }
    }
}
