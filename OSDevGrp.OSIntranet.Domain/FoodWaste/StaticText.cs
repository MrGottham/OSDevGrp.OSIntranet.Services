using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Static text used by the food waste domain.
    /// </summary>
    public class StaticText : TranslatableBase, IStaticText
    {
        #region Private variables

        private StaticTextType _staticTextType;
        private Guid _subjectTranslationIdentifier;
        private Guid? _bodyTranslationIdentifier;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a static text used by the food waste domain.
        /// </summary>
        /// <param name="staticTextType">Type of the static text.</param>
        /// <param name="subjectTranslationIdentifier">Translation identifier for the subject to the static text.</param>
        /// <param name="bodyTranslationIdentifier">Translation identifier for the body to the static text.</param>
        public StaticText(StaticTextType staticTextType, Guid subjectTranslationIdentifier, Guid? bodyTranslationIdentifier = null)
        {
            _staticTextType = staticTextType;
            _subjectTranslationIdentifier = subjectTranslationIdentifier;
            _bodyTranslationIdentifier = bodyTranslationIdentifier;
        }

        /// <summary>
        /// Creates a static text used by the food waste domain.
        /// </summary>
        protected StaticText()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the static text.
        /// </summary>
        public virtual StaticTextType Type
        {
            get { return _staticTextType; }
            protected set { _staticTextType = value; }
        }

        /// <summary>
        /// Gets the translation identifier for the subject to the static text.
        /// </summary>
        public virtual Guid SubjectTranslationIdentifier
        {
            get { return _subjectTranslationIdentifier; }
            protected set { _subjectTranslationIdentifier = value; }
        }

        /// <summary>
        /// Gets the translation for the subject to the static text.
        /// </summary>
        public virtual ITranslation SubjectTranslation { get; private set; }

        /// <summary>
        /// Gets the translations of the subject to the static text.
        /// </summary>
        public virtual IEnumerable<ITranslation> SubjectTranslations
        {
            get { return Translations.Where(m => m.TranslationOfIdentifier == SubjectTranslationIdentifier).ToList(); }
        }

        /// <summary>
        /// Gets the translation identifier for the body to the static text.
        /// </summary>
        public virtual Guid? BodyTranslationIdentifier
        {
            get { return _bodyTranslationIdentifier; }
            protected set { _bodyTranslationIdentifier = value; }
        }

        /// <summary>
        /// Gets the translation for the body to the static text.
        /// </summary>
        public virtual ITranslation BodyTranslation { get; private set; }

        /// <summary>
        /// Gets the translations of the body to the static text.
        /// </summary>
        public virtual IEnumerable<ITranslation> BodyTranslations
        {
            get
            {
                return BodyTranslationIdentifier.HasValue ? Translations.Where(m => m.TranslationOfIdentifier == BodyTranslationIdentifier.Value).ToList() : new List<ITranslation>(0);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finish up the translation for the static text.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        protected override void OnTranslation(CultureInfo translationCulture)
        {
            base.OnTranslation(translationCulture);
            SubjectTranslation = Translate(translationCulture, SubjectTranslationIdentifier);
            BodyTranslation = BodyTranslationIdentifier.HasValue ? Translate(translationCulture, BodyTranslationIdentifier) : null;
        }

        #endregion
    }
}
