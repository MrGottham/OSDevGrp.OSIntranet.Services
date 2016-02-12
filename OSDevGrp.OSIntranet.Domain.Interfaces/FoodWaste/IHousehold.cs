using System.Globalization;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a household.
    /// </summary>
    public interface IHousehold : IIdentifiable
    {
        /// <summary>
        /// Gets or sets the description for the household.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Make translation for the household.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        void Translate(CultureInfo translationCulture);
    }
}
