namespace OSDevGrp.OSIntranet.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface for the Argument Null Guard.
    /// </summary>
    public interface IArgumentNullGuard
    {
        /// <summary>
        /// Validates whether the <paramref name="value"/> is null and throws an <see cref="System.ArgumentNullException"/> when so.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>Instance of Argument Null Guard.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <see cref="value"/> is null or when the <see cref="argumentName"/> is null, empty or white space.</exception>
        IArgumentNullGuard NotNull(object value, string argumentName);

        /// <summary>
        /// Validates whether the <paramref name="value"/> is null or empty and throws an <see cref="System.ArgumentNullException"/> when so.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>Instance of Argument Null Guard.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <see cref="value"/> is null or empty or when the <see cref="argumentName"/> is null, empty or white space.</exception>
        IArgumentNullGuard NotNullOrEmpty(string value, string argumentName);

        /// <summary>
        /// Validates whether the <paramref name="value"/> is null, empty or white space and throws an <see cref="System.ArgumentNullException"/> when so.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>Instance of Argument Null Guard.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <see cref="value"/> is null, empty or white space or when the <see cref="argumentName"/> is null, empty or white space.</exception>
        IArgumentNullGuard NotNullOrWhiteSpace(string value, string argumentName);
    }
}
