using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.QueryHandlers.Core;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
{
    /// <summary>
    /// Tests the basic functionality which can handle a query for getting some data for a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberDataGetQueryHandlerBaseTests
    {
        /// <summary>
        /// Private class for a query which can get some data for a household member.
        /// </summary>
        private class MyHouseholdMemberDataGetQuery : HouseholdMemberDataGetQueryBase
        {
        }

        /// <summary>
        /// Private class for a query which can get some translatable data for a household member.
        /// </summary>
        private class MyHouseholdMemberTranslatableDataGetQuery : HouseholdMemberTranslatableDataGetQueryBase
        {
        }

        /// <summary>
        /// Private class for testing the basic functionality which can handle a query for getting some data for a household member.
        /// </summary>
        /// <typeparam name="TQuery">Type of the query for getting some data for a household member.</typeparam>
        /// <typeparam name="TData">Type of the data which query should select.</typeparam>
        /// <typeparam name="TView">Type of the view which should return the selected data.</typeparam>
        private class MyHouseholdMemberDataGetQueryHandler<TQuery, TData, TView> : HouseholdMemberDataGetQueryHandlerBase<TQuery, TData, TView> where TQuery : HouseholdMemberDataGetQueryBase
        {
        }
    }
}
