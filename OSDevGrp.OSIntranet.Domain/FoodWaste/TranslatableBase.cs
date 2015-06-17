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

        private IList<ITranslation> _translations = new List<ITranslation>(0);
 
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
            protected set { _translations = value == null ? null : value.ToList(); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an translation for the domain object.
        /// </summary>
        /// <param name="translation">Translation for the domain object.</param>
        public virtual void TranslationAdd(ITranslation translation)
        {
            if (translation == null)
            {
                throw new ArgumentNullException("translation");
            }
            _translations.Add(translation);
        }

        /// <summary>
        /// Make translation for the domain object.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        public void Translate(CultureInfo translationCulture)
        {
            if (translationCulture == null)
            {
                throw new ArgumentNullException("translationCulture");
            }
            Translation = Translate(translationCulture, Identifier);
            OnTranslation(translationCulture);
        }

        /// <summary>
        /// Make translation for a given identifier.
        /// </summary>
        /// <param name="translationCulture">>Culture information which are used for translation.</param>
        /// <param name="translationForIdentifier">Identifier for the domain object which should be translated.</param>
        /// <returns>Translation for the given identifier.</returns>
        protected ITranslation Translate(CultureInfo translationCulture, Guid? translationForIdentifier)
        {
            if (translationCulture == null)
            {
                throw new ArgumentNullException("translationCulture");
            }
            if (translationForIdentifier.HasValue == false || Translations == null || Translations.Any() == false)
            {
                return null;
            }
            var translationForIdentifierCollection = Translations.Where(m => m.TranslationOfIdentifier == translationForIdentifier.Value).ToList();
            if (translationForIdentifierCollection.Any() == false)
            {
                return null;
            }
            var translation = translationForIdentifierCollection.SingleOrDefault(m => string.Compare(m.TranslationInfo.CultureInfo.Name, translationCulture.Name, StringComparison.Ordinal) == 0);
            if (translation != null)
            {
                return translation;
            }
            translation = translationForIdentifierCollection.SingleOrDefault(m => string.Compare(m.TranslationInfo.CultureInfo.Name, Thread.CurrentThread.CurrentUICulture.Name, StringComparison.Ordinal) == 0);
            return translation ?? translationForIdentifierCollection.OrderBy(m => m.TranslationInfo.CultureInfo).First();
        }

        /// <summary>
        /// Functionality which are executed when the translatable domain object are translated.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        protected virtual void OnTranslation(CultureInfo translationCulture)
        {
        }

        #endregion
    }
}
