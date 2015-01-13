using System.Globalization;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for translation information which are used for translation.
    /// </summary>
    public interface ITranslationInfo : IIdentifiable
    {
        /// <summary>
        /// Gets the name for the culture which are used for translation.
        /// </summary>
        string CultureName { get; }

        /// <summary>
        /// Gets the culture information which are used for translation.
        /// </summary>
        CultureInfo CultureInfo { get; }
    }
}
