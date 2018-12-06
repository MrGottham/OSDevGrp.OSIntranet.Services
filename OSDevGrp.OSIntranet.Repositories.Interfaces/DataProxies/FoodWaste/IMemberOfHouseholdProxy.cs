using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy which bind a given household member to a given household.
    /// </summary>
    public interface IMemberOfHouseholdProxy : IIdentifiable, IMySqlDataProxy
    {
        /// <summary>
        /// Household member which are member of the household.
        /// </summary>
        IHouseholdMember HouseholdMember { get; }

        /// <summary>
        /// Identifier for the household member which are member of the household.
        /// </summary>
        Guid? HouseholdMemberIdentifier { get; }

        /// <summary>
        /// Household which the household member are member of.
        /// </summary>
        IHousehold Household { get; }

        /// <summary>
        /// Identifier for the household which the household member are member of.
        /// </summary>
        Guid? HouseholdIdentifier { get; }

        /// <summary>
        /// Date and time for when the membership to the household was created.
        /// </summary>
        DateTime CreationTime { get; }
    }
}
