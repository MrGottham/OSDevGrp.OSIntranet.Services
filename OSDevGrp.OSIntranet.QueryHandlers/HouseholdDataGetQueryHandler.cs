using System;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Query handler which handles the query for getting household data for one of the current user households.
    /// </summary>
    public class HouseholdDataGetQueryHandler : HouseholdMemberDataGetQueryHandlerBase<HouseholdDataGetQuery, IHousehold, HouseholdView>
    {
        #region Constructor

        /// <summary>
        /// Creates the query handler which handles the query for getting household data for one of the current user households.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        public HouseholdDataGetQueryHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper)
            : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the household data for one of the current user households.
        /// </summary>
        /// <param name="householdMember">Household member on which to get household data for one of the households.</param>
        /// <param name="query">Query for getting household data for one of the current user households.</param>
        /// <param name="translationInfo">Translation informations which can be used for translation.</param>
        /// <returns>Household.</returns>
        public override IHousehold GetData(IHouseholdMember householdMember, HouseholdDataGetQuery query, ITranslationInfo translationInfo)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            if (translationInfo == null)
            {
                throw new ArgumentNullException("translationInfo");
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}
