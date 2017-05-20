namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a storage type.
    /// </summary>
    public interface IStorageType : ITranslatable
    {
        /// <summary>
        /// Gets the order for sortering storage types.
        /// </summary>
        int SortOrder { get; }

        /// <summary>
        /// Gets the default temperature for the storage type.
        /// </summary>
        int Temperature { get; }

        /// <summary>
        /// Gets the temperature range for the storage type.
        /// </summary>
        IRange<int> TemperatureRange { get; }

        /// <summary>
        /// Gets whether household members can create storages of this type.
        /// </summary>
        bool Creatable { get; }

        /// <summary>
        /// Gets whether household members can edit storages of this type.
        /// </summary>
        bool Editable { get; }

        /// <summary>
        /// Gets whether household members can delete storages of this type.
        /// </summary>
        bool Deletable { get; }
    }
}
