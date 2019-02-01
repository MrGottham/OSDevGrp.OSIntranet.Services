using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for common validations used by domain objects in the food waste domain.
    /// </summary>
    public interface IDomainObjectValidations
    {
        /// <summary>
        /// Validates whether a value is a mail address.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        /// <returns>True if the value is a mail address otherwise false.</returns>
        bool IsMailAddress(string value);

        /// <summary>
        /// Gets the limit of households according to a given membership.
        /// </summary>
        /// <param name="membership">Membership.</param>
        /// <returns>Limit of households according to a given membership.</returns>
        int GetHouseholdLimit(Membership membership);

        /// <summary>
        /// Validates whether the limit of households has been reached according to a given membership.
        /// </summary>
        /// <param name="membership">Membership.</param>
        /// <param name="numberOfHouseholds">Number of households.</param>
        /// <returns>True if the limit of households has been reached otherwise false.</returns>
        bool HasReachedHouseholdLimit(Membership membership, int numberOfHouseholds);

        /// <summary>
        /// Validates whether a given membership matches the required membership.
        /// </summary>
        /// <param name="membership">Membership which should match the required membership.</param>
        /// <param name="requiredMembership">Required membership.</param>
        /// <returns>True if the given membership matches the required membership otherwise false.</returns>
        bool HasRequiredMembership(Membership membership, Membership requiredMembership);

        /// <summary>
        /// Validates whether the current membership can be upgraded to another membership.
        /// </summary>
        /// <param name="currentMembership">Current membership.</param>
        /// <param name="upgradeToMembership">Membership which should be upgraded to.</param>
        /// <returns>True if the current membership can be upgraded to the other membership otherwise false.</returns>
        bool CanUpgradeMembership(Membership currentMembership, Membership upgradeToMembership);

        /// <summary>
        /// Validates whether a value is inside a given range.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="range">The range where the value should be inside.</param>
        /// <returns>True when the value is inside the given range otherwise false.</returns>
        bool InRange(int value, IRange<int> range);

        /// <summary>
        /// Validates whether a storage can be added to an existing storages collection.
        /// </summary>
        /// <param name="storage">The storage to validate.</param>
        /// <param name="existingStorageCollection">The existing storage collection.</param>
        /// <returns>True when the storage can be added to the existing storage collection otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="storage"/> or <paramref name="existingStorageCollection"/> is null.</exception>
        bool CanAddStorage(IStorage storage, IEnumerable<IStorage> existingStorageCollection);

        /// <summary>
        /// Validates whether a storage can be removed.
        /// </summary>
        /// <param name="storage">The storage to validate.</param>
        /// <returns>True when the storage can be removed otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="storage"/> is null.</exception>
        bool CanRemoveStorage(IStorage storage);
    }
}
