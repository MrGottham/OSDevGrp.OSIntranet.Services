using System.Globalization;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a translatable domain object in the food waste domain.
    /// </summary>
    public interface ITranslatable : IIdentifiable
    {
        /// <summary>
        /// Make translation for the domain object.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        void Translate(CultureInfo translationCulture);
    }
}
