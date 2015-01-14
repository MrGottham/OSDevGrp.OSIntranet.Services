using System.Globalization;

namespace OSDevGrp.OSIntranet.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface for an object mapper which can map objects in the food waste domain.
    /// </summary>
    public interface IFoodWasteObjectMapper
    {
        /// <summary>
        /// Maps a source object to a destination object.
        /// </summary>
        /// <typeparam name="TSource">Type for the source object to map.</typeparam>
        /// <typeparam name="TDestination">Type for the destination object.</typeparam>
        /// <param name="source">Source object to map.</param>
        /// <param name="translationCulture">Culture information used to translation.</param>
        /// <returns>Destination object mapped from the source object.</returns>
        TDestination Map<TSource, TDestination>(TSource source, CultureInfo translationCulture = null);
    }
}
