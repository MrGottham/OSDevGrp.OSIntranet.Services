using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers.Dispatchers
{
    /// <summary>
    /// Interface to a dispatcher which can dispatch the welcome letter to a household member.
    /// </summary>
    public interface IWelcomeLetterDispatcher : IDispatcher<IHouseholdMember>
    {
    }
}
