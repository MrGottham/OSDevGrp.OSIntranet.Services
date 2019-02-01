using System;
using System.Globalization;

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

        /// <summary>
        /// Gets or sets the description for the storage.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the temperature for the storage.
        /// </summary>
        int Temperature { get; set; }

        /// <summary>
        /// Gets the creation date and time for when the storage was created.
        /// </summary>
        DateTime CreationTime { get; }

        /// <summary>
        /// Make translation for the storage.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        /// <param name="translateHousehold">Indicates whether the <see cref="Household"/> should be translated.</param>
        /// <param name="translateStorageType">Indicates whether the <see cref="StorageType"/> should be translated.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="translationCulture"/> is null.</exception>
        void Translate(CultureInfo translationCulture, bool translateHousehold, bool translateStorageType = true);
    }
}
