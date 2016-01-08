using System;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.QueryHandlers.Core
{
    /// <summary>
    /// Basic functionality which can handle a query for getting some data for a household member.
    /// </summary>
    /// <typeparam name="TQuery">Type of the query for getting some data for a household member.</typeparam>
    /// <typeparam name="TData">Type of the data which query should select.</typeparam>
    /// <typeparam name="TView">Type of the view which should return the selected data.</typeparam>
    public abstract class HouseholdMemberDataGetQueryHandlerBase<TQuery, TData, TView> : IQueryHandler<TQuery, TView> where TQuery : HouseholdMemberDataGetQueryBase
    {
        #region Private variables

        private readonly IHouseholdDataRepository _householdDataRepository;
        private readonly IClaimValueProvider _claimValueProvider;
        private readonly IFoodWasteObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the basic functionality which can handle a query for getting some data for a household member.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        protected HouseholdMemberDataGetQueryHandlerBase(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper)
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

        #region Properties

        /// <summary>
        /// Gets whether the household member should be activated to get the data for the query handled by this query handler.
        /// </summary>
        public virtual bool ShouldBeActivated
        {
            get { return true; }
        }

        /// <summary>
        /// Gets whether the household member should have accepted the privacy policy to get the data for the query handled by this query handler.
        /// </summary>
        public virtual bool ShouldHaveAcceptedPrivacyPolicy
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the requeired membership which the household member should have to get the data for the query handled by this query handler.
        /// </summary>
        public virtual Membership RequiredMembership
        {
            get { return Membership.Basic; }
        }

        /// <summary>
        /// Gets the repository which can access household data for the food waste domain.
        /// </summary>
        protected virtual IHouseholdDataRepository HouseholdDataRepository
        {
            get { return _householdDataRepository; }
        }

        /// <summary>
        /// Gets the provider which can resolve values from the current users claims.
        /// </summary>
        protected virtual IClaimValueProvider ClaimValueProvider
        {
            get { return _claimValueProvider; }
        }

        /// <summary>
        /// Gets the object mapper which can map objects in the food waste domain.
        /// </summary>
        protected virtual IFoodWasteObjectMapper ObjectMapper
        {
            get { return _objectMapper; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Functionality which can handle a query for getting some data for a household member.
        /// </summary>
        /// <param name="query">Query for getting some data for a household member.</param>
        /// <returns>View of the selected data.</returns>
        public virtual TView Query(TQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var translationInfo = GetTranslationInfo(query);

            var householdMember = HouseholdDataRepository.HouseholdMemberGetByMailAddress(ClaimValueProvider.MailAddress);
            if (householdMember == null)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotCreated));
            }
            if (ShouldBeActivated && householdMember.IsActivated == false)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotActivated));
            }
            if (ShouldHaveAcceptedPrivacyPolicy && householdMember.IsPrivacyPolictyAccepted == false)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberHasNotAcceptedPrivacyPolicy));
            }
            if (householdMember.HasRequiredMembership(RequiredMembership) == false)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberHasNotRequiredMembership));
            }

            var data = GetData(householdMember, query, translationInfo);

            return ObjectMapper.Map<TData, TView>(data, translationInfo == null ? null : translationInfo.CultureInfo);
        }

        /// <summary>
        /// Gets the data for the household member.
        /// </summary>
        /// <param name="householdMember">Household member for which to get the data.</param>
        /// <param name="query">Query for getting some data for a household member.</param>
        /// <param name="translationInfo">Translation informations.</param>
        /// <returns>Data for the household member.</returns>
        public abstract TData GetData(IHouseholdMember householdMember, TQuery query, ITranslationInfo translationInfo);

        /// <summary>
        /// Gets the translation informations which should be used to translate data selected by this query.
        /// </summary>
        /// <param name="query">Query for getting some data for a household member.</param>
        /// <returns>Translation informations which should be used to translate data selected by this query.</returns>
        private ITranslationInfo GetTranslationInfo(TQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var householdMemberTranslatableDataGetQuery = query as HouseholdMemberTranslatableDataGetQueryBase;
            if (householdMemberTranslatableDataGetQuery == null)
            {
                return null;
            }

            var translationInfo = HouseholdDataRepository.Get<ITranslationInfo>(householdMemberTranslatableDataGetQuery.TranslationInfoIdentifier);
            if (translationInfo == null)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, householdMemberTranslatableDataGetQuery.TranslationInfoIdentifier));
            }
            return translationInfo;
        }

        #endregion
    }
}
