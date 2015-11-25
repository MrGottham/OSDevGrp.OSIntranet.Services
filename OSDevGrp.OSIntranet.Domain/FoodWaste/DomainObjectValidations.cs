using System;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Common validations used by domain objects in the food waste domain.
    /// </summary>
    public class DomainObjectValidations : IDomainObjectValidations
    {
        #region Private variables

        private static IDomainObjectValidations _domainObjectValidations;
        private static readonly object SyncRoot = new object();

        #endregion

        /// <summary>
        /// Validates whether a value is a mail address.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        /// <returns>True if the value is a mail address otherwise false.</returns>
        public virtual bool IsMailAddress(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            var regularExpression = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.Compiled);
            return regularExpression.IsMatch(value);
        }

        /// <summary>
        /// Creates a instance of common validations used by domain objects in the food waste domain.
        /// </summary>
        /// <returns>Instance of common validations used by domain objects in the food waste domain.</returns>
        public static IDomainObjectValidations Create()
        {
            lock (SyncRoot)
            {
                return _domainObjectValidations ?? (_domainObjectValidations = new DomainObjectValidations());
            }
        }
    }
}
