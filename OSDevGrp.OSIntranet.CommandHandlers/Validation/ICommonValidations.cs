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
    }
}
