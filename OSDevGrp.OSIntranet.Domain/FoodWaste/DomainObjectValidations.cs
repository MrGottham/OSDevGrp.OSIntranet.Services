using System;
using System.Reflection;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

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

        #region Methods

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
        /// Gets the limit of households according to a given membership.
        /// </summary>
        /// <param name="membership">Membership.</param>
        /// <returns>Limit of households according to a given membership.</returns>
        public virtual int GetHouseholdLimit(Membership membership)
        {
            switch (membership)
            {
                case Membership.Basic:
                    return 1;

                case Membership.Deluxe:
                    return 2;

                case Membership.Premium:
                    return 999;

                default:
                    throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, membership, "membership", MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Validates whether the limit of households has been reached accoing to a given membesrhip.
        /// </summary>
        /// <param name="membership">Membership.</param>
        /// <param name="numberOfHouseholds">Number of households.</param>
        /// <returns>True if the limit of households has been reached otherwise false.</returns>
        public virtual bool HasReachedHouseholdLimit(Membership membership, int numberOfHouseholds)
        {
            var householdLimit = GetHouseholdLimit(membership);
            return numberOfHouseholds >= householdLimit;
        }

        /// <summary>
        /// Validates whether a given membership matches the required membership.
        /// </summary>
        /// <param name="membership">Membership which should match the required membership.</param>
        /// <param name="requiredMembership">Required membership.</param>
        /// <returns>True if the given membership matches the required membership otherwise false.</returns>
        public virtual bool HasRequiredMembership(Membership membership, Membership requiredMembership)
        {
            return (int) membership >= (int) requiredMembership;
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

        #endregion
    }
}
