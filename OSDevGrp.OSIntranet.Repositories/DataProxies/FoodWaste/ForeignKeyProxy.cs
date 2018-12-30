using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
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
        #region Private constants

        private const string CacheName = "OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste.ForeignKeyProxy.Cache";

        #endregion

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

            DataProxyCache.AddDataProxyCollectionToCache(CacheName, this, foreignKeyProxy => foreignKeyProxy.Identifier == Identifier.Value);
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

            DataProxyCache.RemoveDataProxyCollectionToCache(CacheName, this, foreignKeyProxy => foreignKeyProxy.Identifier == Identifier.Value);
        }

        /// <summary>
        /// Creates the SQL statement for getting this foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <returns>SQL statement for getting this foreign key to a domain object in the food waste domain.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return BuildSystemDataCommandForSelecting("WHERE fk.ForeignKeyIdentifier=@foreignKeyIdentifier", systemDataCommandBuilder => systemDataCommandBuilder.AddForeignKeyIdentifierParameter(Identifier));
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

            HashSet<ForeignKeyProxy> cache = DataProxyCache.GetCachedDataProxyCollection<ForeignKeyProxy>(CacheName);
            if (cache.Any(foreignKeyProxy => foreignKeyProxy.ForeignKeyForIdentifier == foreignKeyForIdentifier))
            {
                return cache.Where(foreignKeyProxy => foreignKeyProxy.ForeignKeyForIdentifier == foreignKeyForIdentifier).ToList();
            }

            using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                List<ForeignKeyProxy> result = new List<ForeignKeyProxy>(subDataProvider.GetCollection<ForeignKeyProxy>(BuildSystemDataCommandForSelecting("WHERE fk.ForeignKeyForIdentifier=@foreignKeyForIdentifier", systemDataCommandBuilder => systemDataCommandBuilder.AddForeignKeyForIdentifierParameter(foreignKeyForIdentifier))));
                DataProxyCache.AddDataProxyCollectionToCache(CacheName, result, foreignKeyProxy => foreignKeyProxy.ForeignKeyForIdentifier == foreignKeyForIdentifier);
                return result;
            }
        }

        /// <summary>
        /// Creates a MySQL command selecting a collection of <see cref="ForeignKeyProxy"/>.
        /// </summary>
        /// <param name="whereClause">The WHERE clause which the MySQL command should use.</param>
        /// <param name="parameterAdder">The callback to add MySQL parameters to the MySQL command.</param>
        /// <returns>MySQL command selecting a collection of <see cref="ForeignKeyProxy"/>.</returns>
        internal static MySqlCommand BuildSystemDataCommandForSelecting(string whereClause = null, Action<SystemDataCommandBuilder> parameterAdder = null)
        {
            StringBuilder selectStatementBuilder = new StringBuilder("SELECT fk.ForeignKeyIdentifier,fk.DataProviderIdentifier,dp.Name AS DataProviderName,dp.HandlesPayments,dp.DataSourceStatementIdentifier,fk.ForeignKeyForIdentifier,fk.ForeignKeyForTypes,fk.ForeignKeyValue FROM ForeignKeys AS fk INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=fk.DataProviderIdentifier");
            if (string.IsNullOrWhiteSpace(whereClause) == false)
            {
                selectStatementBuilder.Append($" {whereClause}");
            }

            SystemDataCommandBuilder systemDataCommandBuilder = new SystemDataCommandBuilder(selectStatementBuilder.ToString());
            if (parameterAdder == null)
            {
                return systemDataCommandBuilder.Build();
            }

            parameterAdder(systemDataCommandBuilder);
            return systemDataCommandBuilder.Build();
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
