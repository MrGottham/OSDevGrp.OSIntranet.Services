using System;
using System.Collections.Generic;
using System.Globalization;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Basic functionality for a translatable domain object in the food waste domain.
    /// </summary>
    public abstract class TranslatableBase : IdentifiableBase, ITranslatable
    {
        #region Properties

        /// <summary>
        /// Gets the current translation after Translate has been called.
        /// </summary>
        public virtual ITranslation Translation { get; set; }

        /// <summary>
        /// Gets the translations for the translatable domain object.
        /// </summary>
        public virtual IEnumerable<ITranslation> Translations { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Make translation for the domain object.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        public virtual void Translate(CultureInfo translationCulture)
        {
            if (translationCulture == null)
            {
                throw new ArgumentNullException("translationCulture");
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}
