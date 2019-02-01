using System;
using System.Collections.Generic;
using System.Globalization;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a household.
    /// </summary>
    public interface IHousehold : IIdentifiable
    {
        /// <summary>
        /// Name for the household.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Description for the household.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Date and time for when the household was created.
        /// </summary>
        DateTime CreationTime { get; }

        /// <summary>
        /// Household members who is member of this household.
        /// </summary>
        IEnumerable<IHouseholdMember> HouseholdMembers { get; }

        /// <summary>
        /// Storages in this household.
        /// </summary>
        IEnumerable<IStorage> Storages { get; }

        /// <summary>
        /// Adds a household member to this household.
        /// </summary>
        /// <param name="householdMember">Household member which should be member on this household.</param>
        void HouseholdMemberAdd(IHouseholdMember householdMember);

        /// <summary>
        /// Removes a household member from this household.
        /// </summary>
        /// <param name="householdMember">Household member which should be removed as a member of this household.</param>
        /// <returns>Household member who has been removed af member of this household.</returns>
        IHouseholdMember HouseholdMemberRemove(IHouseholdMember householdMember);

        /// <summary>
        /// Adds a storage to this household.
        /// </summary>
        /// <param name="storage">Storage which should be added to this household.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="storage"/> is null.</exception>
        /// <exception cref="IntranetSystemException">Thrown when the <paramref name="storage"/> cannot be added to this household.</exception>
        void StorageAdd(IStorage storage);

        /// <summary>
        /// Adds a storage to this household.
        /// </summary>
        /// <param name="storage">Storage which should be added to this household.</param>
        /// <param name="householdMember">Household member who tries to add the storage.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="storage"/> or the <paramref name="householdMember"/> is null.</exception>
        /// <exception cref="IntranetBusinessException">Thrown when the <paramref name="householdMember"/> does not have the required membership to add storages to this household.</exception>
        /// <exception cref="IntranetSystemException">Thrown when the <paramref name="storage"/> cannot be added to this household.</exception>
        void StorageAdd(IStorage storage, IHouseholdMember householdMember);

        /// <summary>
        /// Removes a storage from this household.
        /// </summary>
        /// <param name="storage">Storage which should be removed from this household.</param>
        /// <returns>The removed storage.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="storage"/> is null.</exception>
        /// <exception cref="IntranetSystemException">Thrown when the <paramref name="storage"/> cannot be removed from this household.</exception>
        IStorage StorageRemove(IStorage storage);

        /// <summary>
        /// Removes a storage from this household.
        /// </summary>
        /// <param name="storage">Storage which should be removed from this household.</param>
        /// <param name="householdMember">Household member who tries to remove the storage.</param>
        /// <returns>The removed storage.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="storage"/> or the <paramref name="householdMember"/> is null.</exception>
        /// <exception cref="IntranetBusinessException">Thrown when the <paramref name="householdMember"/> does not have the required membership to remove storages from this household.</exception>
        /// <exception cref="IntranetSystemException">Thrown when the <paramref name="storage"/> cannot be removed from this household.</exception>
        IStorage StorageRemove(IStorage storage, IHouseholdMember householdMember);

        /// <summary>
        /// Make translation for the household.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        /// <param name="translateHouseholdMembers">Indicates whether to make translation for all the household members who has a membership on this household.</param>
        /// <param name="translateStorages">Indicates whether to make translation for all the storages on the household.</param>
        void Translate(CultureInfo translationCulture, bool translateHouseholdMembers, bool translateStorages = true);
    }
}
