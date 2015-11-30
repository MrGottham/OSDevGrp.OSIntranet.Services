using System;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy to a given static text used by the food waste domain.
    /// </summary>
    public class StaticTextProxy : StaticText, IStaticTextProxy
    {
        #region Constructors

        /// <summary>
        /// Creates a data proxy to a given static text used by the food waste domain.
        /// </summary>
        public StaticTextProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a given static text used by the food waste domain.
        /// </summary>
        /// <param name="staticTextType">Type of the static text.</param>
        /// <param name="subjectTranslationIdentifier">Translation identifier for the subject to the static text.</param>
        /// <param name="bodyTranslationIdentifier">Translation identifier for the body to the static text.</param>
        public StaticTextProxy(StaticTextType staticTextType, Guid subjectTranslationIdentifier, Guid? bodyTranslationIdentifier = null)
            : base(staticTextType, subjectTranslationIdentifier, bodyTranslationIdentifier)
        {
        }

        #endregion
    }
}
