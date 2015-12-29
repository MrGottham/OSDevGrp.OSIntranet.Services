using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers.Dispatchers
{
    /// <summary>
    /// Interface for a dispatcher which can dispatch data for a given domain object in the food waste domain.
    /// </summary>
    /// <typeparam name="TDomainObject">Type of the domain object which provides data to dispatch.</typeparam>
    public interface IDispatcher<in TDomainObject> where TDomainObject : IIdentifiable
    {
        /// <summary>
        /// Dispatch data for a given domain object in the food waste domain.
        /// </summary>
        /// <param name="stakeholder">Stakeholder which data should be dispatched to.</param>
        /// <param name="domainObject">Domain object which provides data to dispatch.</param>
        void Dispatch(IStakeholder stakeholder, TDomainObject domainObject);
    }
}
