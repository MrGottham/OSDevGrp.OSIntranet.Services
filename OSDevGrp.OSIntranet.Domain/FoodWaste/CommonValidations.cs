using System;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Common validation functionality used by the domain objects in the food waste domain.
    /// </summary>
    public static class CommonValidations
    {
        /// <summary>
        /// Validates whether a value is a mail address.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        /// <returns>True if the value is a mail address otherwise false.</returns>
        public static bool IsMailAddress(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            var regularExpression = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.Compiled);
            return regularExpression.IsMatch(value);
        }
    }
}
