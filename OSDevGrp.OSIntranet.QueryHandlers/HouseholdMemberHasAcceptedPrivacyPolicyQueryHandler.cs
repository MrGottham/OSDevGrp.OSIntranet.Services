using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Query handler which handles the query for checking whether the current user has accepted the privacy policy.
    /// </summary>
    public class HouseholdMemberHasAcceptedPrivacyPolicyQueryHandler : HouseholdMemberDataGetQueryHandlerBase<HouseholdMemberHasAcceptedPrivacyPolicyQuery, bool, BooleanResultResponse>
    {
        #region Constructors

        /// <summary>
        /// Creates the query handler which handles the query for checking whether the current user has accepted the privacy policy.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        public HouseholdMemberHasAcceptedPrivacyPolicyQueryHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper)
            : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether the household member should be activated to get the data for the query handled by this query handler.
        /// </summary>
        public override bool ShouldBeActivated => false;

        /// <summary>
        /// Gets whether the household member should have accepted the privacy policy to get the data for the query handled by this query handler.
        /// </summary>
        public override bool ShouldHaveAcceptedPrivacyPolicy => false;

        #endregion

        #region Methods

        /// <summary>
        /// Gets the data which indicates whether the household member has accepted the privacy policy.
        /// </summary>
        /// <param name="householdMember">Household member.</param>
        /// <param name="query">Query which can check whether the current user has accepted the privacy policy.</param>
        /// <param name="translationInfo">Translation information which can be used for translation.</param>
        /// <returns>Data which indicates whether the household member has accepted the privacy policy.</returns>
        public override bool GetData(IHouseholdMember householdMember, HouseholdMemberHasAcceptedPrivacyPolicyQuery query, ITranslationInfo translationInfo)
        {
            ArgumentNullGuard.NotNull(householdMember, nameof(householdMember));

            return householdMember.IsPrivacyPolicyAccepted;
        }

        #endregion
    }
}
