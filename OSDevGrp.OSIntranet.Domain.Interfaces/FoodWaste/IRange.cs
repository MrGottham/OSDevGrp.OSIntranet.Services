namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a range which can describe an interval.
    /// </summary>
    /// <typeparam name="T">Type for the interval.</typeparam>
    public interface IRange<out T> where T : struct
    {
        /// <summary>
        /// Gets start value for the interval.
        /// </summary>
        T StartValue { get; }

        /// <summary>
        /// Gets the end value for the interval.
        /// </summary>
        T EndValue { get; }
    }
}
