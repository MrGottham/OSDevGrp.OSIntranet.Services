using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Basic functionality for a translatable domain object in the food waste domain.
    /// </summary>
    public abstract class TranslatableBase : IdentifiableBase, ITranslatable
    {
        #region Private variables

        private IEnumerable<ITranslation> _translations = new List<ITranslation>(0);
 
        #endregion

        #region Properties

        /// <summary>
        /// Gets the current translation after Translate has been called.
        /// </summary>
        public virtual ITranslation Translation { get; private set; }

        /// <summary>
        /// Gets the translations for the translatable domain object.
        /// </summary>
        public virtual IEnumerable<ITranslation> Translations
        {
            get { return _translations; }
            protected set { _translations = value; }
        }

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
            if (Translations == null || Translations.Any() == false)
            {
                Translation = null;
                return;
            }
            var translation = Translations.SingleOrDefault(m => string.Compare(m.TranslationInfo.CultureInfo.Name, translationCulture.Name, StringComparison.Ordinal) == 0);
            if (translation != null)
            {
                Translation = translation;
                return;
            }
            translation = Translations.SingleOrDefault(m => string.Compare(m.TranslationInfo.CultureInfo.Name, Thread.CurrentThread.CurrentUICulture.Name, StringComparison.Ordinal) == 0);
            Translation = translation ?? Translations.OrderBy(m => m.TranslationInfo.CultureInfo.Name).First();
        }

        #endregion
    }
}
