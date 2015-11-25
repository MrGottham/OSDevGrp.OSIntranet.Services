namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for common validations used by domain objects in the food waste domain.
    /// </summary>
    public interface ICommonValidations
    {
        /// <summary>
        /// Validates whether a value is a mail address.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        /// <returns>True if the value is a mail address otherwise false.</returns>
        bool IsMailAddress(string value);
    }
}
