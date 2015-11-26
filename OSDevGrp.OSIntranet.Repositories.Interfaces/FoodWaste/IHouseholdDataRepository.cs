using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a repository which can access household data for the food waste domain.
    /// </summary>
    public interface IHouseholdDataRepository : IDataRepository
    {
        /// <summary>
        /// Gets a household member by their mail address.
        /// </summary>
        /// <param name="mailAddress">Mail address for the household member to get.</param>
        /// <returns>Household member when exists; otherwise null.</returns>
        IHouseholdMember HouseholdMemberGetByMailAddress(string mailAddress);
    }
}
