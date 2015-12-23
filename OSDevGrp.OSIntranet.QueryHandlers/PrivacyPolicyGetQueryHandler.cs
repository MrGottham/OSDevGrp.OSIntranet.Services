using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Functionality which handles the query for getting the privacy policy.
    /// </summary>
    public class PrivacyPolicyGetQueryHandler : StaticTextGetQueryHandlerBase<PrivacyPolicyGetQuery>
    {
        #region Constructor

        /// <summary>
        /// Creates the functionality which handles the query for getting the privacy policy.
        /// </summary>
        /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
        public PrivacyPolicyGetQueryHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper) 
            : base(systemDataRepository, foodWasteObjectMapper)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type for the specific static text to get.
        /// </summary>
        public override StaticTextType StaticTextType
        {
            get { return StaticTextType.PrivacyPolicy; }
        }

        #endregion
    }
}
