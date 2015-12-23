using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers.Dispatchers
{
    /// <summary>
    /// Interface to the functionality which can merge fields in a static text.
    /// </summary>
    public interface IStaticTextFieldMerge
    {
        /// <summary>
        /// Gets the fields and values which can be merged.
        /// </summary>
        IEnumerable<KeyValuePair<string, object>> MergeFields { get; }

        /// <summary>
        /// Merge field values into the static text.
        /// </summary>
        /// <param name="staticText">Static text on which to merge field values.</param>
        /// <param name="translationInfo">Translation informations used to translate the static text.</param>
        void Merge(IStaticText staticText, ITranslationInfo translationInfo);
    }
}
