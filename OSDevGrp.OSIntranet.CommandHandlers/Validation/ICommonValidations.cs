using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.CommandHandlers.Validation
{
    /// <summary>
    /// Interface for common validations.
    /// </summary>
    public interface ICommonValidations
    {
        /// <summary>
        /// Checks whether a GUID is legal.
        /// </summary>
        /// <param name="guid">GUID to check.</param>
        /// <returns>True if the GUID is legal otherwise false.</returns>
        bool IsGuidLegal(Guid guid);

        /// <summary>
        /// Checks whether a string has a value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>True if the string has a value otherwise false.</returns>
        bool HasValue(string value);

        /// <summary>
        /// Checks whether a string contains an illegal char.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>True if the string contains an illegal char otherwise false.</returns>
        bool ContainsIllegalChar(string value);

        /// <summary>
        /// Checks whether an object is not null.
        /// </summary>
        /// <param name="value">Object.</param>
        /// <returns>True when the object is not null otherwise false.</returns>
        bool IsNotNull(object value);

        /// <summary>
        /// Checks whether two strings are equal or not.
        /// </summary>
        /// <param name="xValue">String.</param>
        /// <param name="yValue">String.</param>
        /// <param name="comparisonType">Comparison type.</param>
        /// <returns>True when the two string are equal otherwise false.</returns>
        bool Equals(string xValue, string yValue, StringComparison comparisonType = StringComparison.Ordinal);

        /// <summary>
        /// Checks whether a given string value are a legal enum value.
        /// </summary>
        /// <typeparam name="TEnum">Type of the enum.</typeparam>
        /// <param name="value">String value which should be checked.</param>
        /// <param name="legalValues">Legal enum values.</param>
        /// <returns>True when the given string value are a legal enum value otherwise false.</returns>
        bool IsLegalEnumValue<TEnum>(string value, IEnumerable<TEnum> legalValues) where TEnum : struct, IConvertible;

        /// <summary>
        /// Checks whether a given string value are a legal enum value.
        /// </summary>
        /// <typeparam name="TEnum">Type of the enum.</typeparam>
        /// <param name="value">String value which should be checked.</param>
        /// <returns>True when the given string value are a legal enum value otherwise false.</returns>
        bool IsLegalEnumValue<TEnum>(string value) where TEnum : struct, IConvertible;

        /// <summary>
        /// Checks whether a given date and time value is in a given date and time interval.
        /// </summary>
        /// <param name="value">Date and time value which should be in the interval.</param>
        /// <param name="minTime">Start date and time for the interval.</param>
        /// <param name="maxTime">End date and time for the interval.</param>
        /// <returns>True when the given date and time is in the given date and time interval otherwise false.</returns>
        bool IsDateTimeInInterval(DateTime value, DateTime minTime, DateTime maxTime);

        /// <summary>
        /// Checks whether a given date and time value is in the past.
        /// </summary>
        /// <param name="value">Date and time value which should be in the past.</param>
        /// <returns>True when the given date and time is in the past otherwise false.</returns>
        bool IsDateTimeInPast(DateTime value);

        /// <summary>
        /// Checks whether a given date and time value is in the future.
        /// </summary>
        /// <param name="value">Date and time value which should be in the future.</param>
        /// <returns>True when the given date and time is in the future otherwise false.</returns>
        bool IsDateTimeInFuture(DateTime value);

        /// <summary>
        /// Checks whether a given string value has a valid length.
        /// </summary>
        /// <param name="value">String value on which to check the length.</param>
        /// <param name="minLength">Min length for the string value.</param>
        /// <param name="maxLength">Max length for the string value.</param>
        /// <returns>True when the string value has a valid length otherwise false.</returns>
        bool IsLengthValid(string value, int minLength, int maxLength);
    }
}
