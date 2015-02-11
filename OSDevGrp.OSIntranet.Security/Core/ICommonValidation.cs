namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Interface for common validation used by the security logic.
    /// </summary>
    public interface ICommonValidation
    {
        /// <summary>
        /// Validates whether a value is a mail address.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        /// <returns>True if the value is a mail address otherwise false.</returns>
        bool IsMailAddress(string value);
    }
}
