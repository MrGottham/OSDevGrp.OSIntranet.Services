using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a static text used by the food waste domain.
    /// </summary>
    public interface IStaticText : ITranslatable
    {
        /// <summary>
        /// Gets the type of the static text.
        /// </summary>
        StaticTextTypes Type { get; }

        /// <summary>
        /// Gets the translation identifier for the subject to the static text.
        /// </summary>
        Guid SubjectTranslationIdentifier { get; }

        /// <summary>
        /// Gets the translation for the subject to the static text.
        /// </summary>
        ITranslation SubjectTranslation { get; }

        /// <summary>
        /// Gets the translations of the subject to the static text.
        /// </summary>
        IEnumerable<ITranslation> SubjectTranslations { get; }

        /// <summary>
        /// Gets the translation identifier for the body to the static text.
        /// </summary>
        Guid? BodyTranslationIdentifier { get; }

        /// <summary>
        /// Gets the translation for the body to the static text.
        /// </summary>
        ITranslation BodyTranslation { get; }

        /// <summary>
        /// Gets the translations of the body to the static text.
        /// </summary>
        IEnumerable<ITranslation> BodyTranslations { get; }
    }
}
