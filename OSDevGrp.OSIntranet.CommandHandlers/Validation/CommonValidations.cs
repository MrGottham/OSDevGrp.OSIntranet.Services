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

        #endregion
    }
}
