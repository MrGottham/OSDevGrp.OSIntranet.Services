﻿using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy to a given foreign key to a domain object in the food waste domain.
    /// </summary>
    public class ForeignKeyProxy : ForeignKey, IForeignKeyProxy
    {
        #region Constructors

        /// <summary>
        /// Creates a data proxy to a given foreign key to a domain object in the food waste domain.
        /// </summary>
        public ForeignKeyProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a given foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Data provider who own the foreign key.</param>
        /// <param name="foreignKeyForIdentifier">Identifier for the domain object which has this foreign key.</param>
        /// <param name="foreignKeyForType">Type which has this foreign key.</param>
        /// <param name="foreignKeyValue">Value of the foreign key.</param>
        public ForeignKeyProxy(IDataProvider dataProvider, Guid foreignKeyForIdentifier, Type foreignKeyForType, string foreignKeyValue)
            : base(dataProvider, foreignKeyForIdentifier, foreignKeyForType, foreignKeyValue)
        {
        }

        /// <summary>
        /// Creates a data proxy to a given foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Data provider who own the foreign key.</param>
        /// <param name="foreignKeyForIdentifier">Identifier for the domain object which has this foreign key.</param>
        /// <param name="foreignKeyForTypes">Types which has this foreign key.</param>
        /// <param name="foreignKeyValue">Value of the foreign key.</param>
        public ForeignKeyProxy(IDataProvider dataProvider, Guid foreignKeyForIdentifier, IEnumerable<Type> foreignKeyForTypes, string foreignKeyValue)
            : base(dataProvider, foreignKeyForIdentifier, foreignKeyForTypes, foreignKeyValue)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the identifier for the data provider.
        /// </summary>
        private Guid DataProviderIdentifier { get; set; }

        #endregion

        #region IMySqlDataProxy

        /// <summary>
        /// Gets the unique identification for the foreign key.
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
        /// Gets the SQL statement for selecting a given foreign key.
        /// </summary>
        /// <param name="foreignKey">Foreign key for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selection a given foreign key.</returns>
        public virtual string GetSqlQueryForId(IForeignKey foreignKey)
        {
            if (foreignKey == null)
            {
                throw new ArgumentNullException("foreignKey");
            }
            if (foreignKey.Identifier.HasValue)
            {
                return string.Format("SELECT ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue FROM ForeignKeys WHERE ForeignKeyIdentifier='{0}'", foreignKey.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foreignKey.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this foreign key.
        /// </summary>
        /// <returns>SQL statement to insert this foreign key.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            var emptyGuid = Guid.Empty;
            return string.Format("INSERT INTO ForeignKeys (ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue) VALUES('{0}','{1}','{2}','{3}','{4}')", UniqueId, DataProvider.Identifier.HasValue ? DataProvider.Identifier.Value.ToString("D").ToUpper() : emptyGuid.ToString("D").ToUpper(), ForeignKeyForIdentifier, string.Join(";", ForeignKeyForTypes.Select(m => m.Name)), ForeignKeyValue);
        }

        /// <summary>
        /// Gets the SQL statement to update this foreign key.
        /// </summary>
        /// <returns>SQL statement to update this foreign key.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            var emptyGuid = Guid.Empty;
            return string.Format("UPDATE ForeignKeys SET DataProviderIdentifier='{1}',ForeignKeyForIdentifier='{2}',ForeignKeyForTypes='{3}',ForeignKeyValue='{4}' WHERE ForeignKeyIdentifier='{0}'", UniqueId, DataProvider.Identifier.HasValue ? DataProvider.Identifier.Value.ToString("D").ToUpper() : emptyGuid.ToString("D").ToUpper(), ForeignKeyForIdentifier, string.Join(";", ForeignKeyForTypes.Select(m => m.Name)), ForeignKeyValue);
        }

        /// <summary>
        /// Gets the SQL statement to delete this foreign key.
        /// </summary>
        /// <returns>SQL statement to delete this foreign key.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM ForeignKeys WHERE ForeignKeyIdentifier='{0}'", UniqueId);
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            Identifier = new Guid(dataReader.GetString("ForeignKeyIdentifier"));
            ForeignKeyForIdentifier = new Guid(dataReader.GetString("ForeignKeyForIdentifier"));
            ForeignKeyValue = dataReader.GetString("ForeignKeyValue");

            var foreignKeyForTypes = new List<Type>();
            foreach (var foreignKeyForTypeName in dataReader.GetString("ForeignKeyForTypes").Split(';').Where(m => string.IsNullOrWhiteSpace(m) == false))
            {
                // ReSharper disable EmptyGeneralCatchClause
                try
                {
                    var typeName = foreignKeyForTypeName;
                    var assembly = typeof (IDomainObject).Assembly;
                    var type = assembly.GetTypes().SingleOrDefault(m => string.Compare(m.Name, typeName, StringComparison.Ordinal) == 0);
                    if (type != null)
                    {
                        foreignKeyForTypes.Add(type);
                    }
                }
                catch
                {
                    // Don't throw the exception. Just ignore the type.
                }
                // ReSharper restore EmptyGeneralCatchClause
            }
            ForeignKeyForTypes = foreignKeyForTypes;

            DataProviderIdentifier = new Guid(dataReader.GetString("DataProviderIdentifier"));
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                var dataProviderProxy = new DataProviderProxy
                {
                    Identifier = DataProviderIdentifier
                };
                DataProvider = subDataProvider.Get(dataProviderProxy);
            }
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }
        }

        /// <summary>
        /// Creates the SQL statement for getting this foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <returns>SQL statement for getting this foreign key to a domain object in the food waste domain.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <returns>SQL statement for inserting this foreign key to a domain object in the food waste domain.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <returns>SQL statement for updating this foreign key to a domain object in the food waste domain.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <returns>SQL statement for deleting this foreign key to a domain object in the food waste domain.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForDelete()).Build();
        }

        /// <summary>
        /// Gets foreign keys for a domain object in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="foreignKeyForIdentifier">Identifier for the given domain object on which to get the foreign keys.</param>
        /// <returns>Foreign keys for a domain object in the food waste domain.</returns>
        internal static IEnumerable<ForeignKeyProxy> GetDomainObjectForeignKeys(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid foreignKeyForIdentifier)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = new FoodWasteCommandBuilder(DataRepositoryHelper.GetSqlStatementForSelectingForeignKeys(foreignKeyForIdentifier)).Build();
                return subDataProvider.GetCollection<ForeignKeyProxy>(command);
            }
        }

        /// <summary>
        /// Deletes foreign keys for a domain object in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="foreignKeyForIdentifier">Identifier for the given domain object on which to get the foreign keys.</param>
        internal static void DeleteDomainObjectForeignKeys(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid foreignKeyForIdentifier)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            foreach (var foreignKeyProxy in GetDomainObjectForeignKeys(dataProvider, foreignKeyForIdentifier))
            {
                using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(foreignKeyProxy);
                }
            }
        }

        #endregion
    }
}
