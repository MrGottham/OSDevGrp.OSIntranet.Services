using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
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
    public class SystemDataRepository : DataRepositoryBase, ISystemDataRepository
    {
        #region Constructor

        /// <summary>
        /// Creates a repository which can access system data for the food waste domain.
        /// </summary>
        /// <param name="foodWasteDataProvider">Implementation of a data provider which can access data in the food waste repository.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        public SystemDataRepository(IFoodWasteDataProvider foodWasteDataProvider, IFoodWasteObjectMapper foodWasteObjectMapper)
            : base(foodWasteDataProvider, foodWasteObjectMapper)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all the translations for a given domain object.
        /// </summary>
        /// <param name="identifiableDomainObject">The identifiable domain object on which all the translations should be returned.</param>
        /// <returns>All translations for the given domain object.</returns>
        public virtual IEnumerable<ITranslation> TranslationsForDomainObjectGet(IIdentifiable identifiableDomainObject)
        {
            if (identifiableDomainObject == null)
            {
                throw new ArgumentNullException("identifiableDomainObject");
            }
            try
            {
                if (identifiableDomainObject.Identifier.HasValue)
                {
                    return DataProvider.GetCollection<TranslationProxy>(string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.OfIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier ORDER BY CultureName", identifiableDomainObject.Identifier.Value.ToString().ToUpper()));
                }
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, identifiableDomainObject.Identifier, "Identifier"));
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "TranslationsForDomainObjectGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Gets all the translation informations which can be used for translation.
        /// </summary>
        /// <returns>All the translation informations which can be used for translation.</returns>
        public virtual IEnumerable<ITranslationInfo> TranslationInfoGetAll()
        {
            try
            {
                return DataProvider.GetCollection<TranslationInfoProxy>("SELECT TranslationInfoIdentifier,CultureName FROM TranslationInfos ORDER BY CultureName");
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
