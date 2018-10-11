using System;
using System.Collections.Generic;
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
    /// Data proxy to a given data provider.
    /// </summary>
    public class DataProviderProxy : DataProvider, IDataProviderProxy
    {
        #region Private variables

        private bool _translationCollectionHasBeenLoaded;
        private IFoodWasteDataProvider _dataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a data proxy to a given data provider.
        /// </summary>
        public DataProviderProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a given data provider.
        /// </summary>
        /// <param name="name">Name for the data provider.</param>
        /// <param name="handlesPayments">Indication of whether the data provider handles payments.</param>
        /// <param name="dataSourceStatementIdentifier">Identifier for the data source statement.</param>
        public DataProviderProxy(string name, bool handlesPayments, Guid dataSourceStatementIdentifier)
            : base(name, handlesPayments, dataSourceStatementIdentifier)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the translations for the data provider.
        /// </summary>
        public override IEnumerable<ITranslation> Translations
        {
            get
            {
                if (_translationCollectionHasBeenLoaded || _dataProvider == null)
                {
                    return base.Translations;
                }

                Translations = new List<ITranslation>(TranslationProxy.GetDomainObjectTranslations(_dataProvider, DataSourceStatementIdentifier));
                _translationCollectionHasBeenLoaded = true;

                return base.Translations;
            }
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Gets the unique identification for the data provider.
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

            Identifier = GetDataProviderIdentifier(dataReader, "DataProviderIdentifier");
            Name = GetDataProviderName(dataReader, "Name");
            HandlesPayments = GetHandlesPayments(dataReader, "HandlesPayments");
            DataSourceStatementIdentifier = GetDataSourceStatementIdentifier(dataReader, "DataSourceStatementIdentifier");

            _translationCollectionHasBeenLoaded = false;
            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            Translations = new List<ITranslation>(TranslationProxy.GetDomainObjectTranslations(dataProvider, DataSourceStatementIdentifier));

            _translationCollectionHasBeenLoaded = true;
            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Creates the SQL statement for getting this data provider.
        /// </summary>
        /// <returns>SQL statement for getting this data provider.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new SystemDataCommandBuilder("SELECT DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier FROM DataProviders WHERE DataProviderIdentifier=@dataProviderIdentifier")
                .AddDataProviderIdentifierParameter(Identifier)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this data provider.
        /// </summary>
        /// <returns>SQL statement for inserting this data provider.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new SystemDataCommandBuilder("INSERT INTO DataProviders (DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier) VALUES(@dataProviderIdentifier,@name,@handlesPayments,@dataSourceStatementIdentifier)")
                .AddDataProviderIdentifierParameter(Identifier)
                .AddDataProviderNameParameter(Name)
                .AddHandlesPaymentsParameter(HandlesPayments)
                .AddDataSourceStatementIdentifierParameter(DataSourceStatementIdentifier)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this data provider.
        /// </summary>
        /// <returns>SQL statement for updating this data provider.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new SystemDataCommandBuilder("UPDATE DataProviders SET Name=@name,HandlesPayments=@handlesPayments,DataSourceStatementIdentifier=@dataSourceStatementIdentifier WHERE DataProviderIdentifier=@dataProviderIdentifier")
                .AddDataProviderIdentifierParameter(Identifier)
                .AddDataProviderNameParameter(Name)
                .AddHandlesPaymentsParameter(HandlesPayments)
                .AddDataSourceStatementIdentifierParameter(DataSourceStatementIdentifier)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this data provider.
        /// </summary>
        /// <returns>SQL statement for deleting this data provider.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new SystemDataCommandBuilder("DELETE FROM DataProviders WHERE DataProviderIdentifier=@dataProviderIdentifier")
                .AddDataProviderIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region IMySqlDataProxyCreator<IDataProviderProxy> Members

        /// <summary>
        /// Creates an instance of the data proxy to a given data provider with values from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="dataProvider">Data provider which supports the data reader.</param>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the data proxy to a given data provider with values from the data reader.</returns>
        public virtual IDataProviderProxy Create(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, params string[] columnNameCollection)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider))
                .NotNull(columnNameCollection, nameof(columnNameCollection));

            return new DataProviderProxy(
                GetDataProviderName(dataReader, columnNameCollection[1]),
                GetHandlesPayments(dataReader, columnNameCollection[2]),
                GetDataSourceStatementIdentifier(dataReader, columnNameCollection[3]))
            {
                Identifier = GetDataProviderIdentifier(dataReader, columnNameCollection[0])
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the data provider identifier from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The data provider identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static Guid GetDataProviderIdentifier(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return new Guid(dataReader.GetString(columnName));
        }

        /// <summary>
        /// Gets the name of the data provider from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The name of the data provider.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static string GetDataProviderName(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetString(columnName);
        }

        /// <summary>
        /// Gets whether the data provider handles payments from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>Whether the data provider handles payments.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static bool GetHandlesPayments(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return Convert.ToBoolean(dataReader.GetInt32(columnName));
        }

        /// <summary>
        /// Gets the data source statement identifier from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The data source statement identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static Guid GetDataSourceStatementIdentifier(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return new Guid(dataReader.GetString(columnName));
        }

        #endregion
    }
}
