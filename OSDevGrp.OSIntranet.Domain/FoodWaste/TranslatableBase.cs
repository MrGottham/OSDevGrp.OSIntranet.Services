using System;
using System.Globalization;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Basic functionality for a translatable domain object in the food waste domain.
    /// </summary>
    public abstract class TranslatableBase : IdentifiableBase, ITranslatable
    {
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
