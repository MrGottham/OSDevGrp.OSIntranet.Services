using System;
using System.Globalization;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Translation information which are used for translation.
    /// </summary>
    public class TranslationInfo : IdentifiableBase, ITranslationInfo
    {
        #region Private variables

        private readonly string _cultureName;
        private readonly CultureInfo _cultureInfo;

        #endregion

        #region Constructor

        /// <summary>
        /// Create translation information which are used for translation for a given culture.
        /// </summary>
        /// <param name="cultureName">Name for the culture on which the translation information should be based.</param>
        public TranslationInfo(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName))
            {
                throw new ArgumentNullException("cultureName");
            }
            _cultureName = cultureName;
            _cultureInfo = new CultureInfo(_cultureName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name for the culture which are used for translation.
        /// </summary>
        public virtual string CultureName
        {
            get { return _cultureName; }
        }

        /// <summary>
        /// Gets the culture information which are used for translation.
        /// </summary>
        public virtual CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
        }

        #endregion
    }
}
