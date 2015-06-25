using System;
using System.Collections.Generic;
using System.Reflection;
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
        #region Private variables

        private readonly Guid _dataProviderForFoodsIdentifier = new Guid("5A1B9283-6406-44DF-91C5-F2FB83CC9A42");
        private readonly Guid _dataProviderForFoodGroupsIdentifier = new Guid("5A1B9283-6406-44DF-91C5-F2FB83CC9A42");

        #endregion

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
        /// Gets all the foreign keys for a given domain object.
        /// </summary>
        /// <param name="identifiableDomainObject">The identifiable domain object on which all the foreign keys should be returned.</param>
        /// <returns>All the foreign keys for the given domain object.</returns>
        public virtual IEnumerable<IForeignKey> ForeignKeysForDomainObjectGet(IIdentifiable identifiableDomainObject)
        {
            if (identifiableDomainObject == null)
            {
                throw new ArgumentNullException("identifiableDomainObject");
            }
            try
            {
                if (identifiableDomainObject.Identifier.HasValue)
                {
                    return DataProvider.GetCollection<ForeignKeyProxy>(DataRepositoryHelper.GetSqlStatementForSelectingForeignKeys(identifiableDomainObject.Identifier.Value));
                }
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, identifiableDomainObject.Identifier, "Identifier"));
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }

        /// <summary>
        /// Gets the default data provider for foods.
        /// </summary>
        /// <returns>Default data provider for foods</returns>
        public virtual IDataProvider DataProviderForFoodsGet()
        {
            try
            {
                var dataProviderProxy = new DataProviderProxy
                {
                    Identifier = _dataProviderForFoodsIdentifier
                };
                return DataProvider.Get(dataProviderProxy);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }

        /// <summary>
        /// Gets the default data provider for food groups.
        /// </summary>
        /// <returns>Default data provider for food groups.</returns>
        public virtual IDataProvider DataProviderForFoodGroupsGet()
        {
            try
            {
                var dataProviderProxy = new DataProviderProxy
                {
                    Identifier = _dataProviderForFoodGroupsIdentifier
                };
                return DataProvider.Get(dataProviderProxy);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }

        /// <summary>
        /// Gets all the data providers.
        /// </summary>
        /// <returns>All the data providers.</returns>
        public virtual IEnumerable<IDataProvider> DataProviderGetAll()
        {
            try
            {
                return DataProvider.GetCollection<DataProviderProxy>("SELECT DataProviderIdentifier,Name,DataSourceStatementIdentifier FROM DataProviders ORDER BY Name");
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }

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
                    return DataProvider.GetCollection<TranslationProxy>(DataRepositoryHelper.GetSqlStatementForSelectingTranslations(identifiableDomainObject.Identifier.Value));
                }
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, identifiableDomainObject.Identifier, "Identifier"));
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
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
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }

        #endregion
    }
}
