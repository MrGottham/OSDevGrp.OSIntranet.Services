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
    /// Query handler which handles the query for getting household member data for the current user.
    /// </summary>
    public class HouseholdMemberDataGetQueryHandler : HouseholdMemberDataGetQueryHandlerBase<HouseholdMemberDataGetQuery, IHouseholdMember, HouseholdMemberView>
    {
        #region Constructor

        /// <summary>
        /// Creates the query handler which handles the query for getting household member data for the current user.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        public HouseholdMemberDataGetQueryHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper)
            : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the household member data for the current user.
        /// </summary>
        /// <param name="householdMember">Household member for which to get household member ddata.</param>
        /// <param name="query">Query for getting household member data for the current user.</param>
        /// <param name="translationInfo">Translation informations which can be used for translation.</param>
        /// <returns>Household member data for the current user.</returns>
        public override IHouseholdMember GetData(IHouseholdMember householdMember, HouseholdMemberDataGetQuery query, ITranslationInfo translationInfo)
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

            householdMember.Translate(translationInfo.CultureInfo, false);

            return householdMember;
        }

        #endregion
    }
}
