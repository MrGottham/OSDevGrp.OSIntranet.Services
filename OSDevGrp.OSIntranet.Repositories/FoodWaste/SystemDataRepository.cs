using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.FoodWaste
{
    /// <summary>
    /// Repository which can access system data for the food waste domain.
    /// </summary>
    public class SystemDataRepository : ISystemDataRepository
    {
        #region Private variables

        private readonly IFoodWasteDataProvider _foodWasteDataProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a repository which can access system data for the food waste domain.
        /// </summary>
        /// <param name="foodWasteDataProvider">Implementation of a data provider which can access data in the food waste repository.</param>
        public SystemDataRepository(IFoodWasteDataProvider foodWasteDataProvider)
        {
            if (foodWasteDataProvider == null)
            {
                throw new ArgumentNullException("foodWasteDataProvider");
            }
            _foodWasteDataProvider = foodWasteDataProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all the translation informations which can be used for translation.
        /// </summary>
        /// <returns>All the translation informations which can be used for translation.</returns>
        public virtual IEnumerable<ITranslationInfo> TranslationInfoGetAll()
        {
            try
            {
                return _foodWasteDataProvider.GetCollection<TranslationInfoProxy>("SELECT TranslationInfoIdentifier,CultureName FROM TranslationInfos ORDER BY CultureName");
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "TranslationInfoGetAll", ex.Message), ex);
            }
        }

        #endregion
    }
}
