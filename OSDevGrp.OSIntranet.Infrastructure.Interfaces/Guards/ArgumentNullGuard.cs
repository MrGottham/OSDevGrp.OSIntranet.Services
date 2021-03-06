﻿using System;

namespace OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards
{
    /// <summary>
    /// Argument Null Guard.
    /// </summary>
    public class ArgumentNullGuard : IArgumentNullGuard
    {
        #region Methods

        /// <summary>
        /// Validates whether the <paramref name="value"/> is null and throws an <see cref="ArgumentNullException"/> when so.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>Instance of Argument Null Guard.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="value"/> is null or when the <see cref="argumentName"/> is null, empty or white space.</exception>
        IArgumentNullGuard IArgumentNullGuard.NotNull(object value, string argumentName)
        {
            return Validate(value, argumentName, v => Equals(v, null));
        }

        /// <summary>
        /// Validates whether the <paramref name="value"/> is null or empty and throws an <see cref="ArgumentNullException"/> when so.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>Instance of Argument Null Guard.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="value"/> is null or empty or when the <see cref="argumentName"/> is null, empty or white space.</exception>
        IArgumentNullGuard IArgumentNullGuard.NotNullOrEmpty(string value, string argumentName)
        {
            return Validate(value, argumentName, string.IsNullOrEmpty);
        }

        /// <summary>
        /// Validates whether the <paramref name="value"/> is null, empty or white space and throws an <see cref="ArgumentNullException"/> when so.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>Instance of Argument Null Guard.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="value"/> is null, empty or white space or when the <see cref="argumentName"/> is null, empty or white space.</exception>
        IArgumentNullGuard IArgumentNullGuard.NotNullOrWhiteSpace(string value, string argumentName)
        {
            return Validate(value, argumentName, string.IsNullOrWhiteSpace);
        }

        /// <summary>
        /// Validates whether the <paramref name="validationPredicate"/> is true and throws an <see cref="ArgumentNullException"/> when so.
        /// </summary>
        /// <typeparam name="T">Type of the value which should be validated.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="validationPredicate">The validation predicate.</param>
        /// <returns>Instance of Argument Null Guard.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="argumentName"/> is null, empty or white space or when the <paramref name="validationPredicate"/> is null or has been validated as true.</exception>
        private IArgumentNullGuard Validate<T>(T value, string argumentName, Predicate<T> validationPredicate)
        {
            if (string.IsNullOrWhiteSpace(argumentName))
            {
                throw new ArgumentNullException(nameof(argumentName));
            }
            if (validationPredicate == null)
            {
                throw new ArgumentNullException(nameof(validationPredicate));
            }

            if (validationPredicate(value))
            {
                throw new ArgumentNullException(argumentName);
            }

            return this;
        }

        /// <summary>
        /// Validates whether the <paramref name="value"/> is null and throws an <see cref="ArgumentNullException"/> when so.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>Instance of Argument Null Guard.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="value"/> is null or when the <see cref="argumentName"/> is null, empty or white space.</exception>
        public static IArgumentNullGuard NotNull(object value, string argumentName)
        {
            IArgumentNullGuard argumentNullGuard = Create();
            return argumentNullGuard.NotNull(value, argumentName);
        }

        /// <summary>
        /// Validates whether the <paramref name="value"/> is null or empty and throws an <see cref="ArgumentNullException"/> when so.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>Instance of Argument Null Guard.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="value"/> is null or empty or when the <see cref="argumentName"/> is null, empty or white space.</exception>
        public static IArgumentNullGuard NotNullOrEmpty(string value, string argumentName)
        {
            IArgumentNullGuard argumentNullGuard = Create();
            return argumentNullGuard.NotNullOrEmpty(value, argumentName);
        }

        /// <summary>
        /// Validates whether the <paramref name="value"/> is null, empty or white space and throws an <see cref="ArgumentNullException"/> when so.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>Instance of Argument Null Guard.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="value"/> is null, empty or white space or when the <see cref="argumentName"/> is null, empty or white space.</exception>
        public static IArgumentNullGuard NotNullOrWhiteSpace(string value, string argumentName)
        {
            IArgumentNullGuard argumentNullGuard = Create();
            return argumentNullGuard.NotNullOrWhiteSpace(value, argumentName);
        }

        /// <summary>
        /// Creates an instance of the Argument Null Guard.
        /// </summary>
        /// <returns>Instance of the Argument Null Guard.</returns>
        private static IArgumentNullGuard Create()
        {
            return new ArgumentNullGuard();
        }

        #endregion
    }
}
