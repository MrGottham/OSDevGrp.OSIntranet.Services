using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy to a given food group.
    /// </summary>
    public class FoodGroupProxy : FoodGroup, IFoodGroupProxy
    {
        #region Private variables

        private Guid? _parentIdentifier;
        private bool _childrenIsLoaded;
        private bool _translationsIsLoaded;
        private bool _foreignKeysIsLoaded;
        private IDataProviderBase<MySqlCommand> _dataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a data proxy to a given food group.
        /// </summary>
        public FoodGroupProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a given food group.
        /// </summary>
        /// <param name="children">Foods groups which has this food group as a parent. </param>
        public FoodGroupProxy(IEnumerable<IFoodGroup> children) 
            : base(children)
        {
            _childrenIsLoaded = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Food group which has this food group as a child.
        /// </summary>
        public override IFoodGroup Parent
        {
            get
            {
                if (base.Parent != null || _parentIdentifier.HasValue == false || _dataProvider == null)
                {
                    return base.Parent;
                }
                using (var subDataProvider = (IDataProviderBase<MySqlCommand>) _dataProvider.Clone())
                {
                    var foodGroupProxy = new FoodGroupProxy
                    {
                        Identifier = _parentIdentifier.Value
                    };
                    base.Parent = subDataProvider.Get(foodGroupProxy);
                }
                return base.Parent;
            }
            set
            {
                base.Parent = value;
                if (value == null && _parentIdentifier.HasValue)
                {
                    _parentIdentifier = null;
                }
            }
        }

        /// <summary>
        /// Foods groups which has this food group as a parent.
        /// </summary>
        public override IEnumerable<IFoodGroup> Children
        {
            get
            {
                if (_childrenIsLoaded || _dataProvider == null)
                {
                    return base.Children;
                }
                base.Children = new List<IFoodGroup>(GetFoodGroupChildren(_dataProvider, Guid.Parse(UniqueId)));
                _childrenIsLoaded = true;
                return base.Children;
            }
        }

        /// <summary>
        /// Translations for the food group.
        /// </summary>
        public override IEnumerable<ITranslation> Translations
        {
            get
            {
                if (_translationsIsLoaded || _dataProvider == null)
                {
                    return base.Translations;
                }
                base.Translations = new List<ITranslation>(TranslationProxy.GetDomainObjectTranslations(_dataProvider, Guid.Parse(UniqueId)));
                _translationsIsLoaded = true;
                return base.Translations;
            }
        }

        /// <summary>
        /// Foreign keys for the food group.
        /// </summary>
        public override IEnumerable<IForeignKey> ForeignKeys
        {
            get
            {
                if (_foreignKeysIsLoaded || _dataProvider == null)
                {
                    return base.ForeignKeys;
                }
                base.ForeignKeys = new List<IForeignKey>(ForeignKeyProxy.GetDomainObjectForeignKeys(_dataProvider, Guid.Parse(UniqueId)));
                _foreignKeysIsLoaded = true;
                return base.ForeignKeys;
            }
        }

        #endregion

        #region IMySqlDataProxy

        /// <summary>
        /// Gets the unique identification for the food group.
        /// </summary>
        public virtual string UniqueId
        {
            get
            {
                if (Identifier.HasValue)
                {
                    return Identifier.Value.ToString("D").ToUpper();
                }
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }
        }

        /// <summary>
        /// Gets the SQL statement for selecting a given food group.
        /// </summary>
        /// <param name="foodGroup">Food group for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selection a given food group.</returns>
        public virtual string GetSqlQueryForId(IFoodGroup foodGroup)
        {
            if (foodGroup == null)
            {
                throw new ArgumentNullException("foodGroup");
            }
            if (foodGroup.Identifier.HasValue)
            {
                return string.Format("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups WHERE FoodGroupIdentifier='{0}'", foodGroup.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroup.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this food group.
        /// </summary>
        /// <returns>SQL statement to insert this food group.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            var parentIdentifier = "NULL";
            if (Parent != null && Parent.Identifier.HasValue)
            {
                parentIdentifier = string.Format("'{0}'", Parent.Identifier.Value.ToString("D").ToUpper());
            }
            return string.Format("INSERT INTO FoodGroups (FoodGroupIdentifier,ParentIdentifier,IsActive) VALUES('{0}',{1},{2})", UniqueId, parentIdentifier, Convert.ToInt32(IsActive));
        }

        /// <summary>
        /// Gets the SQL statement to update this food group.
        /// </summary>
        /// <returns>SQL statement to update this food group</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            var parentIdentifier = "NULL";
            if (Parent != null && Parent.Identifier.HasValue)
            {
                parentIdentifier = string.Format("'{0}'", Parent.Identifier.Value.ToString("D").ToUpper());
            }
            return string.Format("UPDATE FoodGroups SET ParentIdentifier={1},IsActive={2} WHERE FoodGroupIdentifier='{0}'", UniqueId, parentIdentifier, Convert.ToInt32(IsActive));
        }

        /// <summary>
        /// Gets the SQL statement to delete this food group.
        /// </summary>
        /// <returns>SQL statement to delete this food group.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM FoodGroups WHERE FoodGroupIdentifier='{0}'", UniqueId);
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(object dataReader, IDataProviderBase<MySqlCommand> dataProvider)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            var mySqlDataReader = dataReader as MySqlDataReader;
            if (mySqlDataReader == null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, "dataReader", dataReader.GetType().Name));
            }

            Identifier = new Guid(mySqlDataReader.GetString("FoodGroupIdentifier"));
            IsActive = Convert.ToBoolean(mySqlDataReader.GetInt32("IsActive"));

            _parentIdentifier = null;
            var parentIdentifierOrdinal = mySqlDataReader.GetOrdinal("ParentIdentifier");
            if (mySqlDataReader.IsDBNull(parentIdentifierOrdinal) == false)
            {
                _parentIdentifier = string.IsNullOrWhiteSpace(mySqlDataReader.GetString(parentIdentifierOrdinal)) ? (Guid?) null : new Guid(mySqlDataReader.GetString(parentIdentifierOrdinal));
            }

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlCommand> dataProvider, bool isInserting)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }

            var children = GetFoodGroupChildren(dataProvider, Identifier.Value).ToArray();
            foreach (var child in children)
            {
                using (var subDataProvider = (IDataProviderBase<MySqlCommand>) dataProvider.Clone())
                {
                    subDataProvider.Delete(child);
                }
            }
            
            FoodItemGroupProxy.DeleteFoodItemGroups(dataProvider, this);
            TranslationProxy.DeleteDomainObjectTranslations(dataProvider, Identifier.Value);
            ForeignKeyProxy.DeleteDomainObjectForeignKeys(dataProvider, Identifier.Value);
        }

        /// <summary>
        /// Creates the SQL statement for getting this food group.
        /// </summary>
        /// <returns>SQL statement for getting this food group.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this food group.
        /// </summary>
        /// <returns>SQL statement for inserting this food group.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this food group.
        /// </summary>
        /// <returns>SQL statement for updating this food group.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this food group.
        /// </summary>
        /// <returns>SQL statement for deleting this food group.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForDelete()).Build();
        }

        /// <summary>
        /// Gets foods groups which has this a given food group as a parent.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="foodGroupIdentifier">Identifier for the food group which is the parent.</param>
        /// <returns>Foods groups which has this a given food group as a parent.</returns>
        private static IEnumerable<FoodGroupProxy> GetFoodGroupChildren(IDataProviderBase<MySqlCommand> dataProvider, Guid foodGroupIdentifier)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            using (var subDataProvider = (IDataProviderBase<MySqlCommand>) dataProvider.Clone())
            {
                MySqlCommand command = new FoodWasteCommandBuilder(string.Format("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups WHERE ParentIdentifier='{0}'", foodGroupIdentifier.ToString("D").ToUpper())).Build();
                return subDataProvider.GetCollection<FoodGroupProxy>(command);
            }
        }

        #endregion
    }
}
