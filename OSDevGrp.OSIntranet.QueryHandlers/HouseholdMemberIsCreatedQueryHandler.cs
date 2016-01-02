using System;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Query handler which handles the query which can check whether the current user has been created as a household member.
    /// </summary>
    public class HouseholdMemberIsCreatedQueryHandler : IQueryHandler<HouseholdMemberIsCreatedQuery, BooleanResultResponse>
    {
        #region Private variables

        private readonly IHouseholdDataRepository _householdDataRepository;
        private readonly IClaimValueProvider _claimValueProvider;
        private readonly IFoodWasteObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a query handler which handles the query which can check whether the current user has been created as a household member.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        public HouseholdMemberIsCreatedQueryHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper)
        {
            if (householdDataRepository == null)
            {
                throw new ArgumentNullException("householdDataRepository");
            }
            if (claimValueProvider == null)
            {
                throw new ArgumentNullException("claimValueProvider");
            }
            if (foodWasteObjectMapper == null)
            {
                throw new ArgumentNullException("foodWasteObjectMapper");
            }
            _householdDataRepository = householdDataRepository;
            _claimValueProvider = claimValueProvider;
            _objectMapper = foodWasteObjectMapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the query which can check whether the current user has been created as a household member.
        /// </summary>
        /// <param name="query">Query which can check whether the current user has been created as a household member.</param>
        /// <returns>Boolean result.</returns>
        public virtual BooleanResultResponse Query(HouseholdMemberIsCreatedQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var householdMember = _householdDataRepository.HouseholdMemberGetByMailAddress(_claimValueProvider.MailAddress);

            return _objectMapper.Map<bool, BooleanResultResponse>(householdMember != null);
        }

        #endregion
    }
}
