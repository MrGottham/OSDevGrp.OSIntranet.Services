using System.Collections.Generic;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a translatable domain object in the food waste domain.
    /// </summary>
    public interface ITranslatable : IIdentifiable
    {
        /// <summary>
        /// Gets the current translation after Translate has been called.
        /// </summary>
        ITranslation Translation { get; }

        /// <summary>
        /// Gets the translations for the translatable domain object.
        /// </summary>
        IEnumerable<ITranslation> Translations { get; }

        /// <summary>
        /// Adds an translation for the domain object.
        /// </summary>
        /// <param name="translation">Translation for the domain object.</param>
        void TranslationAdd(ITranslation translation);

        /// <summary>
        /// Make translation for the domain object.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        void Translate(CultureInfo translationCulture);
    }
}
