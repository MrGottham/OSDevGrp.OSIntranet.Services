using System;
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
        /// Make translation for the household.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        void Translate(CultureInfo translationCulture);
    }
}
