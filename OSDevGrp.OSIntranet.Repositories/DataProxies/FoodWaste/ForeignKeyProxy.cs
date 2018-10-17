using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
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

        #region IMySqlDataProxy Members

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

        #endregion

        #region IDataProxyBase<MySqlDataReader, MySqlCommand> Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider));

            Identifier = new Guid(dataReader.GetString("ForeignKeyIdentifier"));
            DataProvider = dataProvider.Create(new DataProviderProxy(), dataReader, "DataProviderIdentifier", "DataProviderName", "HandlesPayments", "DataSourceStatementIdentifier");
            ForeignKeyForIdentifier = new Guid(dataReader.GetString("ForeignKeyForIdentifier"));
            ForeignKeyValue = dataReader.GetString("ForeignKeyValue");

            IList<Type> foreignKeyForTypes = new List<Type>();
            foreach (var foreignKeyForTypeName in dataReader.GetString("ForeignKeyForTypes").Split(';').Where(m => string.IsNullOrWhiteSpace(m) == false))
            {
                // ReSharper disable EmptyGeneralCatchClause
                try
                {
                    var typeName = foreignKeyForTypeName;
                    var assembly = typeof(IDomainObject).Assembly;
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

//            DataProviderIdentifier = new Guid(dataReader.GetString("DataProviderIdentifier"));
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

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
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

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
            return new SystemDataCommandBuilder("SELECT fk.ForeignKeyIdentifier,fk.DataProviderIdentifier,dp.Name AS DataProviderName,dp.HandlesPayments,dp.DataSourceStatementIdentifier,fk.ForeignKeyForIdentifier,fk.ForeignKeyForTypes,fk.ForeignKeyValue FROM ForeignKeys AS fk INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=fk.DataProviderIdentifier WHERE fk.ForeignKeyIdentifier=@foreignKeyIdentifier")
                .AddForeignKeyIdentifierParameter(Identifier)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <returns>SQL statement for inserting this foreign key to a domain object in the food waste domain.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new SystemDataCommandBuilder("INSERT INTO ForeignKeys (ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue) VALUES(@foreignKeyIdentifier,@dataProviderIdentifier,@foreignKeyForIdentifier,@foreignKeyForTypes,@foreignKeyValue)")
                .AddForeignKeyIdentifierParameter(Identifier)
                .AddDataProviderIdentifierParameter(DataProvider.Identifier)
                .AddForeignKeyForIdentifierParameter(ForeignKeyForIdentifier)
                .AddForeignKeyForTypesParameter(ForeignKeyForTypes)
                .AddForeignKeyValueParameter(ForeignKeyValue)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <returns>SQL statement for updating this foreign key to a domain object in the food waste domain.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new SystemDataCommandBuilder("UPDATE ForeignKeys SET DataProviderIdentifier=@dataProviderIdentifier,ForeignKeyForIdentifier=@foreignKeyForIdentifier,ForeignKeyForTypes=@foreignKeyForTypes,ForeignKeyValue=@foreignKeyValue WHERE ForeignKeyIdentifier=@foreignKeyIdentifier")
                .AddForeignKeyIdentifierParameter(Identifier)
                .AddDataProviderIdentifierParameter(DataProvider.Identifier)
                .AddForeignKeyForIdentifierParameter(ForeignKeyForIdentifier)
                .AddForeignKeyForTypesParameter(ForeignKeyForTypes)
                .AddForeignKeyValueParameter(ForeignKeyValue)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <returns>SQL statement for deleting this foreign key to a domain object in the food waste domain.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new SystemDataCommandBuilder("DELETE FROM ForeignKeys WHERE ForeignKeyIdentifier=@foreignKeyIdentifier")
                .AddForeignKeyIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets foreign keys for a domain object in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="foreignKeyForIdentifier">Identifier for the given domain object on which to get the foreign keys.</param>
        /// <returns>Foreign keys for a domain object in the food waste domain.</returns>
        internal static IEnumerable<ForeignKeyProxy> GetDomainObjectForeignKeys(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid foreignKeyForIdentifier)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                return subDataProvider.GetCollection<ForeignKeyProxy>(DataRepositoryHelper.GetSqlCommandForSelectingForeignKeys(foreignKeyForIdentifier));
            }
        }

        /// <summary>
        /// Deletes foreign keys for a domain object in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="foreignKeyForIdentifier">Identifier for the given domain object on which to get the foreign keys.</param>
        internal static void DeleteDomainObjectForeignKeys(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid foreignKeyForIdentifier)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            foreach (IForeignKeyProxy foreignKeyProxy in GetDomainObjectForeignKeys(dataProvider, foreignKeyForIdentifier))
            {
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(foreignKeyProxy);
                }
            }
        }

        #endregion
    }
}
