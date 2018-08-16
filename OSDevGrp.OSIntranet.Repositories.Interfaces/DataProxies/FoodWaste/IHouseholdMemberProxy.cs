using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to a household member.
    /// </summary>
    public interface IHouseholdMemberProxy : IHouseholdMember, IMySqlDataProxy
    {
    }
}
