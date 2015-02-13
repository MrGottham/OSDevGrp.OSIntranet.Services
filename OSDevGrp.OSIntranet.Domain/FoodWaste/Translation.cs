using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Translation.
    /// </summary>
    public class Translation : IdentifiableBase, ITranslation
    {
        #region Private variables

        private Guid _translationOfIdentifier;
        private ITranslationInfo _translationInfo;
        private string _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor used by the data providers.
        /// </summary>
        protected Translation()
        {
        }

        /// <summary>
        /// Creates a translation.
        /// </summary>
        /// <param name="translationOfIdentifier">Identifier for the domain object which name can be translated by this object.</param>
        /// <param name="translationInfo">Translation informations used to translate the name for a domain object.</param>
        /// <param name="value">Value which is the translated name for the domain object.</param>
        public Translation(Guid translationOfIdentifier, ITranslationInfo translationInfo, string value)
        {
            if (translationInfo == null)
            {
                throw new ArgumentNullException("translationInfo");
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            _translationOfIdentifier = translationOfIdentifier;
            _translationInfo = translationInfo;
            _value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the identifier for the domain object which name can be translated by this object.
        /// </summary>
        public virtual Guid TranslationOfIdentifier
        {
            get { return _translationOfIdentifier; }
            protected set { _translationOfIdentifier = value; }
        }

        /// <summary>
        /// Gets the translation informations used to translate the name for a domain object.
        /// </summary>
        public virtual ITranslationInfo TranslationInfo
        {
            get { return _translationInfo; }
            protected set { _translationInfo = value; }
        }

        /// <summary>
        /// Gets or sets the value which is the translated name for the domain object.
        /// </summary>
        public virtual string Value
        {
            get { return _value; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                _value = value;
            }
        }

        #endregion
    }
}
