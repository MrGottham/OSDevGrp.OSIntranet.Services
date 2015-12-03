﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
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

        private readonly Guid _dataProviderForFoodItemsIdentifier = new Guid("5A1B9283-6406-44DF-91C5-F2FB83CC9A42");
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
        /// Gets all the food items.
        /// </summary>
        /// <returns>All food items.</returns>
        public virtual IEnumerable<IFoodItem> FoodItemGetAll()
        {
            try
            {
                return DataProvider.GetCollection<FoodItemProxy>("SELECT FoodItemIdentifier,IsActive FROM FoodItems");
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
        /// Gets all the food items which belongs to a given food group.
        /// </summary>
        /// <param name="foodGroup">Food group which the food items should belong to.</param>
        /// <returns>All food items which belongs to the given food group.</returns>
        public virtual IEnumerable<IFoodItem> FoodItemGetAllForFoodGroup(IFoodGroup foodGroup)
        {
            if (foodGroup == null)
            {
                throw new ArgumentNullException("foodGroup");
            }
            try
            {
                if (foodGroup.Identifier.HasValue)
                {
                    return DataProvider.GetCollection<FoodItemProxy>(string.Format("SELECT fi.FoodItemIdentifier AS FoodItemIdentifier,fi.IsActive AS IsActive FROM FoodItems AS fi, FoodItemGroups AS fig WHERE fig.FoodItemIdentifier=fi.FoodItemIdentifier AND fig.FoodGroupIdentifier='{0}'", foodGroup.Identifier.Value.ToString("D").ToUpper()));
                }
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroup.Identifier, "Identifier"));
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
        /// Gets a food item by a given data providers foreign key.
        /// </summary>
        /// <param name="dataProvider">Data provider.</param>
        /// <param name="foreignKeyValue">Foreign key value.</param>
        /// <returns>Food item.</returns>
        public virtual IFoodItem FoodItemGetByForeignKey(IDataProvider dataProvider, string foreignKeyValue)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (string.IsNullOrEmpty(foreignKeyValue))
            {
                throw new ArgumentNullException("foreignKeyValue");
            }
            try
            {
                if (dataProvider.Identifier.HasValue)
                {
                    return DataProvider.GetCollection<FoodItemProxy>(string.Format("SELECT fi.FoodItemIdentifier AS FoodItemIdentifier,fi.IsActive AS IsActive FROM FoodItems AS fi, ForeignKeys AS fk WHERE fi.FoodItemIdentifier=fk.ForeignKeyForIdentifier AND fk.DataProviderIdentifier='{0}' AND fk.ForeignKeyForTypes LIKE '%{1}%' AND fk.ForeignKeyValue='{2}'", dataProvider.Identifier.Value.ToString("D").ToUpper(), typeof (IFoodItem).Name, foreignKeyValue)).FirstOrDefault();
                }
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, dataProvider.Identifier, "Identifier"));
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
        /// Gets all the food groups.
        /// </summary>
        /// <returns>All food groups.</returns>
        public virtual IEnumerable<IFoodGroup> FoodGroupGetAll()
        {
            try
            {
                return DataProvider.GetCollection<FoodGroupProxy>("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups");
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
        /// Gets all the food groups at the root.
        /// </summary>
        /// <returns>All food groups at the root.</returns>
        public virtual IEnumerable<IFoodGroup> FoodGroupGetAllOnRoot()
        {
            try
            {
                return DataProvider.GetCollection<FoodGroupProxy>("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups WHERE ParentIdentifier IS NULL");
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
        /// Gets a food group by a given data providers foreign key.
        /// </summary>
        /// <param name="dataProvider">Data provider.</param>
        /// <param name="foreignKeyValue">Foreign key value.</param>
        /// <returns>Food group.</returns>
        public virtual IFoodGroup FoodGroupGetByForeignKey(IDataProvider dataProvider, string foreignKeyValue)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (string.IsNullOrEmpty(foreignKeyValue))
            {
                throw new ArgumentNullException("foreignKeyValue");
            }
            try
            {
                if (dataProvider.Identifier.HasValue)
                {
                    return DataProvider.GetCollection<FoodGroupProxy>(string.Format("SELECT fg.FoodGroupIdentifier AS FoodGroupIdentifier,fg.ParentIdentifier AS ParentIdentifier,fg.IsActive AS IsActive FROM FoodGroups AS fg, ForeignKeys AS fk WHERE fg.FoodGroupIdentifier=fk.ForeignKeyForIdentifier AND fk.DataProviderIdentifier='{0}' AND fk.ForeignKeyForTypes LIKE '%{1}%' AND fk.ForeignKeyValue='{2}'", dataProvider.Identifier.Value.ToString("D").ToUpper(), typeof (IFoodGroup).Name, foreignKeyValue)).FirstOrDefault();
                }
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, dataProvider.Identifier, "Identifier"));
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
        /// Gets a static text by a given static text type.
        /// </summary>
        /// <param name="staticTextType">Static text type for which to get the static text.</param>
        /// <returns>Static text.</returns>
        public virtual IStaticText StaticTextGetByStaticTextType(StaticTextType staticTextType)
        {
            try
            {
                var staticText = DataProvider.GetCollection<StaticTextProxy>(string.Format("SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts WHERE StaticTextType={0}", (int)staticTextType)).SingleOrDefault(m => m.Type == staticTextType);
                if (staticText == null)
                {
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (StaticTextProxy).Name, staticTextType));
                }
                return staticText;
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
        /// Gets the default data provider for food items.
        /// </summary>
        /// <returns>Default data provider for food items</returns>
        public virtual IDataProvider DataProviderForFoodItemsGet()
        {
            try
            {
                var dataProviderProxy = new DataProviderProxy
                {
                    Identifier = _dataProviderForFoodItemsIdentifier
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
