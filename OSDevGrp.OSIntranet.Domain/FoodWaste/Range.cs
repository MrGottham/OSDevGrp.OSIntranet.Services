using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Range which can describe an interval.
    /// </summary>
    /// <typeparam name="T">Type for the interval.</typeparam>
    public class Range<T> : IRange<T> where T : struct
    {
        #region Constructor

        /// <summary>
        /// Creates a range which can describe an interval.
        /// </summary>
        /// <param name="startValue">Start value for the interval.</param>
        /// <param name="endValue">End value for the interval.</param>
        public Range(T startValue, T endValue)
        {
            StartValue = startValue;
            EndValue = endValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets start value for the interval.
        /// </summary>
        public virtual T StartValue { get; }

        /// <summary>
        /// Gets the end value for the interval.
        /// </summary>
        public virtual T EndValue { get; }

        #endregion
    }
}
