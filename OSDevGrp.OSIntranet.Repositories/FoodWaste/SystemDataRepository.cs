using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
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
        /// Gets all the storage types.
        /// </summary>
        /// <returns>All storage types.</returns>
        public virtual IEnumerable<IStorageType> StorageTypeGetAll()
        {
            try
            {
                MySqlCommand command = new FoodWasteCommandBuilder("SELECT StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable FROM StorageTypes ORDER BY SortOrder").Build();
                return DataProvider.GetCollection<StorageTypeProxy>(command);
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
        /// Gets all the food items.
        /// </summary>
        /// <returns>All food items.</returns>
        public virtual IEnumerable<IFoodItem> FoodItemGetAll()
        {
            try
            {
                MySqlCommand command = new FoodWasteCommandBuilder("SELECT FoodItemIdentifier,IsActive FROM FoodItems").Build();
                return DataProvider.GetCollection<FoodItemProxy>(command);
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
                throw new ArgumentNullException(nameof(foodGroup));
            }
            try
            {
                if (foodGroup.Identifier.HasValue)
                {
                    MySqlCommand command = new FoodWasteCommandBuilder($"SELECT fi.FoodItemIdentifier AS FoodItemIdentifier,fi.IsActive AS IsActive FROM FoodItems AS fi, FoodItemGroups AS fig WHERE fig.FoodItemIdentifier=fi.FoodItemIdentifier AND fig.FoodGroupIdentifier='{foodGroup.Identifier.Value.ToString("D").ToUpper()}'").Build();
                    return DataProvider.GetCollection<FoodItemProxy>(command);
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
                throw new ArgumentNullException(nameof(dataProvider));
            }
            if (string.IsNullOrEmpty(foreignKeyValue))
            {
                throw new ArgumentNullException(nameof(foreignKeyValue));
            }
            try
            {
                if (dataProvider.Identifier.HasValue)
                {
                    MySqlCommand command = new FoodWasteCommandBuilder($"SELECT fi.FoodItemIdentifier AS FoodItemIdentifier,fi.IsActive AS IsActive FROM FoodItems AS fi, ForeignKeys AS fk WHERE fi.FoodItemIdentifier=fk.ForeignKeyForIdentifier AND fk.DataProviderIdentifier='{dataProvider.Identifier.Value.ToString("D").ToUpper()}' AND fk.ForeignKeyForTypes LIKE '%{typeof(IFoodItem).Name}%' AND fk.ForeignKeyValue='{foreignKeyValue}'").Build();
                    return DataProvider.GetCollection<FoodItemProxy>(command).FirstOrDefault();
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
                MySqlCommand command = new FoodWasteCommandBuilder("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups").Build();
                return DataProvider.GetCollection<FoodGroupProxy>(command);
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
                MySqlCommand command = new FoodWasteCommandBuilder("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups WHERE ParentIdentifier IS NULL").Build();
                return DataProvider.GetCollection<FoodGroupProxy>(command);
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
                throw new ArgumentNullException(nameof(dataProvider));
            }
            if (string.IsNullOrEmpty(foreignKeyValue))
            {
                throw new ArgumentNullException(nameof(foreignKeyValue));
            }
            try
            {
                if (dataProvider.Identifier.HasValue)
                {
                    MySqlCommand command = new FoodWasteCommandBuilder($"SELECT fg.FoodGroupIdentifier AS FoodGroupIdentifier,fg.ParentIdentifier AS ParentIdentifier,fg.IsActive AS IsActive FROM FoodGroups AS fg, ForeignKeys AS fk WHERE fg.FoodGroupIdentifier=fk.ForeignKeyForIdentifier AND fk.DataProviderIdentifier='{dataProvider.Identifier.Value.ToString("D").ToUpper()}' AND fk.ForeignKeyForTypes LIKE '%{typeof(IFoodGroup).Name}%' AND fk.ForeignKeyValue='{foreignKeyValue}'").Build();
                    return DataProvider.GetCollection<FoodGroupProxy>(command).FirstOrDefault();
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
                throw new ArgumentNullException(nameof(identifiableDomainObject));
            }
            try
            {
                if (identifiableDomainObject.Identifier.HasValue)
                {
                    MySqlCommand command = new FoodWasteCommandBuilder(DataRepositoryHelper.GetSqlStatementForSelectingForeignKeys(identifiableDomainObject.Identifier.Value)).Build();
                    return DataProvider.GetCollection<ForeignKeyProxy>(command);
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
                MySqlCommand command = new SystemDataCommandBuilder("SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts WHERE StaticTextType=@staticTextType")
                    .AddStaticTextTypeIdentifierParameter(staticTextType)
                    .Build();
                var staticText = DataProvider.GetCollection<StaticTextProxy>(command).SingleOrDefault(m => m.Type == staticTextType);
                if (staticText == null)
                {
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (IStaticText).Name, staticTextType));
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
        /// Gets all the static texts.
        /// </summary>
        /// <returns>All the static texts.</returns>
        public virtual IEnumerable<IStaticText> StaticTextGetAll()
        {
            try
            {
                MySqlCommand command = new SystemDataCommandBuilder("SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts ORDER BY StaticTextType").Build();
                return DataProvider.GetCollection<StaticTextProxy>(command);
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
                MySqlCommand command = new SystemDataCommandBuilder("SELECT DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier FROM DataProviders ORDER BY Name").Build();
                return DataProvider.GetCollection<DataProviderProxy>(command);
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
        /// Gets all the data providers who handles payments.
        /// </summary>
        /// <returns>All the data providers who handles payments.</returns>
        public virtual IEnumerable<IDataProvider> DataProviderWhoHandlesPaymentsGetAll()
        {
            try
            {
                MySqlCommand command = new SystemDataCommandBuilder("SELECT DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier FROM DataProviders WHERE HandlesPayments=1 ORDER BY Name").Build();
                return DataProvider.GetCollection<DataProviderProxy>(command);
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
            ArgumentNullGuard.NotNull(identifiableDomainObject, nameof(identifiableDomainObject));

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
                MySqlCommand command = new SystemDataCommandBuilder("SELECT TranslationInfoIdentifier,CultureName FROM TranslationInfos ORDER BY CultureName").Build();
                return DataProvider.GetCollection<TranslationInfoProxy>(command);
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
