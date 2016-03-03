using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers.Validation
{
    /// <summary>
    /// Common validations.
    /// </summary>
    public class CommonValidations : ICommonValidations
    {
        #region Public variables

        public static readonly IEnumerable<char> IllegalChars = new List<char> {'\'', '"'};

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether a GUID is legal.
        /// </summary>
        /// <param name="guid">GUID to check.</param>
        /// <returns>True if the GUID is legal otherwise false.</returns>
        public virtual bool IsGuidLegal(Guid guid)
        {
            return guid != Guid.Empty;
        }

        /// <summary>
        /// Checks whether a string has a value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>True if the string has a value otherwise false.</returns>
        public virtual bool HasValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) == false;
        }

        /// <summary>
        /// Checks whether a string contains an illegal char.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>True if the string contains an illegal char otherwise false.</returns>
        public virtual bool ContainsIllegalChar(string value)
        {
            if (HasValue(value) == false)
            {
                return false;
            }
            return IllegalChars.Any(illegalChar => value.Contains(Convert.ToString(illegalChar)));
        }

        /// <summary>
        /// Checks whether an object is not null.
        /// </summary>
        /// <param name="value">Object.</param>
        /// <returns>True when the object is not null otherwise false.</returns>
        public virtual bool IsNotNull(object value)
        {
            return value != null;
        }

        /// <summary>
        /// Checks whether two strings are equal or not.
        /// </summary>
        /// <param name="xValue">String.</param>
        /// <param name="yValue">String.</param>
        /// <param name="comparisonType">Comparison type.</param>
        /// <returns>True when the two string are equal otherwise false.</returns>
        public virtual bool Equals(string xValue, string yValue, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return string.Compare(xValue, yValue, comparisonType) == 0;
        }

        /// <summary>
        /// Checks whether a given string value are a legal enum value.
        /// </summary>
        /// <typeparam name="TEnum">Type of the enum.</typeparam>
        /// <param name="value">String value which should be checked.</param>
        /// <param name="legalValues">Legal enum values.</param>
        /// <returns>True when the given string value are a legal enum value otherwise false.</returns>
        public virtual bool IsLegalEnumValue<TEnum>(string value, IEnumerable<TEnum> legalValues) where TEnum : struct, IConvertible
        {
            if (legalValues == null)
            {
                throw new ArgumentNullException("legalValues");
            }
            if (typeof (TEnum).IsEnum == false)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.GenericTypeHasInvalidType, "TEnum", typeof (TEnum).Name));
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            TEnum enumValue;
            if (Enum.TryParse(value, out enumValue))
            {
                return legalValues.Contains(enumValue);
            }
            return false;
        }

        /// <summary>
        /// Checks whether a given string value are a legal enum value.
        /// </summary>
        /// <typeparam name="TEnum">Type of the enum.</typeparam>
        /// <param name="value">String value which should be checked.</param>
        /// <returns>True when the given string value are a legal enum value otherwise false.</returns>
        public virtual bool IsLegalEnumValue<TEnum>(string value) where TEnum : struct, IConvertible
        {
            if (typeof (TEnum).IsEnum == false)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.GenericTypeHasInvalidType, "TEnum", typeof (TEnum).Name));
            }
            return IsLegalEnumValue(value, Enum.GetValues(typeof (TEnum)).Cast<TEnum>().ToArray());
        }

        /// <summary>
        /// Checks whether a given date and time value is in a given date and time interval.
        /// </summary>
        /// <param name="value">Date and time value which should be in the interval.</param>
        /// <param name="minTime">Start date and time for the interval.</param>
        /// <param name="maxTime">End date and time for the interval.</param>
        /// <returns>True when the given date and time is in the given date and time interval otherwise false.</returns>
        public virtual bool IsDateTimeInInterval(DateTime value, DateTime minTime, DateTime maxTime)
        {
            var minTimeUtc = minTime.ToUniversalTime();
            var maxTimeUtc = maxTime.ToUniversalTime();

            return (minTimeUtc.CompareTo(value.ToUniversalTime()) <= 0) && (maxTimeUtc.CompareTo(value.ToUniversalTime()) >= 0);
        }

        /// <summary>
        /// Checks whether a given date and time value is in the past.
        /// </summary>
        /// <param name="value">Date and time value which should be in the past.</param>
        /// <returns>True when the given date and time is in the past otherwise false.</returns>
        public virtual bool IsDateTimeInPast(DateTime value)
        {
            return IsDateTimeInInterval(value, DateTime.MinValue, DateTime.Now);
        }

        /// <summary>
        /// Checks whether a given date and time value is in the future.
        /// </summary>
        /// <param name="value">Date and time value which should be in the future.</param>
        /// <returns>True when the given date and time is in the future otherwise false.</returns>
        public virtual bool IsDateTimeInFuture(DateTime value)
        {
            return IsDateTimeInInterval(value, DateTime.Now, DateTime.MaxValue);
        }

        /// <summary>
        /// Checks whether a given string value has a valid length.
        /// </summary>
        /// <param name="value">String value on which to check the length.</param>
        /// <param name="minLength">Min length for the string value.</param>
        /// <param name="maxLength">Max length for the string value.</param>
        /// <returns>True when the string value has a valid length otherwise false.</returns>
        public virtual bool IsLengthValid(string value, int minLength, int maxLength)
        {
            if (value == null)
            {
                return false;
            }
            return (value.Length >= minLength) && (value.Length <= maxLength);
        }
        
        #endregion
    }
}
