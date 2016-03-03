﻿using System;
using System.Collections.Generic;
using System.Globalization;

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
        /// Make translation for the household.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        /// <param name="translateHouseholdMembers">Indicates whether to make translation for all the household members who has a membership on this household.</param>
        void Translate(CultureInfo translationCulture, bool translateHouseholdMembers);
    }
}
