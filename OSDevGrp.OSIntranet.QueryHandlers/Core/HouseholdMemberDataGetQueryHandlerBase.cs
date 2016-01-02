using OSDevGrp.OSIntranet.Contracts.Queries;

namespace OSDevGrp.OSIntranet.QueryHandlers.Core
{
    /// <summary>
    /// Basic functionality which can handle a query for getting some data for a household member.
    /// </summary>
    /// <typeparam name="TQuery">Type of the query for getting some data for a household member.</typeparam>
    /// <typeparam name="TData">Type of the data which query should select.</typeparam>
    /// <typeparam name="TView">Type of the view which should return the selected data.</typeparam>
    public abstract class HouseholdMemberDataGetQueryHandlerBase<TQuery, TData, TView> where TQuery : HouseholdMemberDataGetQueryBase
    {
    }
}
