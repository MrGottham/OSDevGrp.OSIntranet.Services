using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a translation.
    /// </summary>
    public interface ITranslation : IIdentifiable
    {
        /// <summary>
        /// Gets the identifier for the domain object which name can be translated by this object.
        /// </summary>
        Guid TranslationOfIdentifier { get; }

        /// <summary>
        /// Gets the translation informations used to translate the name for a domain object.
        /// </summary>
        ITranslationInfo TranslationInfo { get; }

        /// <summary>
        /// Gets or sets the value which is the translated name for the domain object.
        /// </summary>
        string Value { get; set; }
    }
}
