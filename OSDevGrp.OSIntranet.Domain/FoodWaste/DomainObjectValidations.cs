using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
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
            ArgumentNullGuard.NotNullOrWhiteSpace(value, nameof(value));

            Regex regularExpression = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.Compiled);
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
        /// Validates whether the limit of households has been reached according to a given membership.
        /// </summary>
        /// <param name="membership">Membership.</param>
        /// <param name="numberOfHouseholds">Number of households.</param>
        /// <returns>True if the limit of households has been reached otherwise false.</returns>
        public virtual bool HasReachedHouseholdLimit(Membership membership, int numberOfHouseholds)
        {
            int householdLimit = GetHouseholdLimit(membership);
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
        /// Validates whether the current membership can be upgraded to another membership.
        /// </summary>
        /// <param name="currentMembership">Current membership.</param>
        /// <param name="upgradeToMembership">Membership which should be upgraded to.</param>
        /// <returns>True if the current membership can be upgraded to the other membership otherwise false.</returns>
        public virtual bool CanUpgradeMembership(Membership currentMembership, Membership upgradeToMembership)
        {
            return (int) currentMembership <= (int) upgradeToMembership;
        }

        /// <summary>
        /// Validates whether a value is inside a given range.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="range">The range where the value should be inside.</param>
        /// <returns>True when the value is inside the given range otherwise false.</returns>
        public virtual bool InRange(int value, IRange<int> range)
        {
            ArgumentNullGuard.NotNull(range, nameof(range));

            return value >= range.StartValue && value <= range.EndValue;
        }

        /// <summary>
        /// Validates whether a storage can be added to an existing storages collection.
        /// </summary>
        /// <param name="storage">The storage to validate.</param>
        /// <param name="existingStorageCollection">The existing storage collection.</param>
        /// <returns>True when the storage can be added to the existing storage collection otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="storage"/> or <paramref name="existingStorageCollection"/> is null.</exception>
        public virtual bool CanAddStorage(IStorage storage, IEnumerable<IStorage> existingStorageCollection)
        {
            ArgumentNullGuard.NotNull(storage, nameof(storage))
                .NotNull(existingStorageCollection, nameof(existingStorageCollection));

            IStorageType storageType = storage.StorageType;
            if (storageType == null)
            {
                return false;
            }

            if (storageType.Creatable)
            {
                return true;
            }

            return existingStorageCollection.Any(m => m.StorageType != null && m.StorageType.Identifier == storageType.Identifier) == false;
        }

        /// <summary>
        /// Validates whether a storage can be removed.
        /// </summary>
        /// <param name="storage">The storage to validate.</param>
        /// <returns>True when the storage can be removed otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="storage"/> is null.</exception>
        public virtual bool CanRemoveStorage(IStorage storage)
        {
            ArgumentNullGuard.NotNull(storage, nameof(storage));

            IStorageType storageType = storage.StorageType;
            if (storageType == null)
            {
                return true;
            }

            return storageType.Deletable;
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
