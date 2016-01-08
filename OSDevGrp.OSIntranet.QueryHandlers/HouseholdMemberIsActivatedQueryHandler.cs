using System;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Query handler which handles the query for checking whether the current user has been activated.
    /// </summary>
    public class HouseholdMemberIsActivatedQueryHandler : HouseholdMemberDataGetQueryHandlerBase<HouseholdMemberIsActivatedQuery, bool, BooleanResultResponse>
    {
        #region Constructor

        /// <summary>
        /// Creates the query handler which handles the query for checking whether the current user has been activated.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        public HouseholdMemberIsActivatedQueryHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper)
            : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether the household member should be activated to get the data for the query handled by this query handler.
        /// </summary>
        public override bool ShouldBeActivated
        {
            get { return false; }
        }

        /// <summary>
        /// Gets whether the household member should have accepted the privacy policy to get the data for the query handled by this query handler.
        /// </summary>
        public override bool ShouldHaveAcceptedPrivacyPolicy
        {
            get { return false; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the data which indicates whether the household member has been activated.
        /// </summary>
        /// <param name="householdMember">Household member.</param>
        /// <param name="query">Query which can check whether the current user has been activated.</param>
        /// <param name="translationInfo">Translation informations which can be used for translation.</param>
        /// <returns>Data which indicates whether the household member has been activated.</returns>
        public override bool GetData(IHouseholdMember householdMember, HouseholdMemberIsActivatedQuery query, ITranslationInfo translationInfo)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }
            return householdMember.IsActivated;
        }

        #endregion
    }
}
