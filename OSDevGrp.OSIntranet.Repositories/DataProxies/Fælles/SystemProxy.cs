using System;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Fælles;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles
{
    /// <summary>
    /// Data proxy for et system under OSWEBDB.
    /// </summary>
    public class SystemProxy : Domain.Fælles.System, ISystemProxy
    {
        #region Constructors

        /// <summary>
        /// Danner en data proxy for et system under OSWEBDB.
        /// </summary>
        public SystemProxy()
            : this(0)
        {
        }

        /// <summary>
        /// Danner en data proxy for et system under OSWEBDB.
        /// </summary>
        /// <param name="nummer">Unik identifikation af systemet under OSWEBDB.</param>
        public SystemProxy(int nummer)
            : this(nummer, typeof(Domain.Fælles.System).Name)
        {
        }

        /// <summary>
        /// Danner en data proxy for et system under OSWEBDB.
        /// </summary>
        /// <param name="nummer">Unik identifikation af systemet under OSWEBDB.</param>
        /// <param name="title">Titel for systemet under OSWEBDB.</param>
        /// <param name="properties">Egenskaber for systemet under OSWEBDB.</param>
        public SystemProxy(int nummer, string title, int properties = 0)
            : base(nummer, title, properties)
        {
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Returnerer den unikke identifikation af systemet under OSWEBDB.
        /// </summary>
        public virtual string UniqueId => Convert.ToString(Nummer);

        #endregion

        #region IDataProxyBase<MySqlDataReader, MySqlCommand> Members

        /// <summary>
        /// Mapper data for et system under OSWEBDB.
        /// </summary>
        /// <param name="dataReader">Datareader.</param>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider));

            Nummer = GetSystemNo(dataReader, "SystemNo");
            Titel = GetTitle(dataReader, "Title");
            Properties = GetProperties(dataReader, "Properties");
        }

        /// <summary>
        /// Mappper relationer til et system under OSWEBDB.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
        }

        /// <summary>
        /// Gemmer relationer til et system under OSWEBDB.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        /// <param name="isInserting">Angivelse af, om der indsættes eller opdateres.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
        }

        /// <summary>
        /// Sletter relationer til et system under OSWEBDB.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
        }

        /// <summary>
        /// Creates the SQL statement for getting this system within OSWEBDB.
        /// </summary>
        /// <returns>SQL statement for getting this system within OSWEBDB.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new CommonCommandBuilder("SELECT SystemNo,Title,Properties FROM Systems WHERE SystemNo=@systemNo")
                .AddSystemNoParameter(Nummer)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this system within OSWEBDB.
        /// </summary>
        /// <returns>SQL statement for inserting this system within OSWEBDB.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new CommonCommandBuilder("INSERT INTO Systems (SystemNo,Title,Properties) VALUES(@systemNo,@title,@properties)")
                .AddSystemNoParameter(Nummer)
                .AddTitleParameter(Titel)
                .AddPropertiesParameter(Properties)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this system within OSWEBDB.
        /// </summary>
        /// <returns>SQL statement for updating this system within OSWEBDB.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new CommonCommandBuilder("UPDATE Systems SET Title=@title,Properties=@properties WHERE SystemNo=@systemNo")
                .AddSystemNoParameter(Nummer)
                .AddTitleParameter(Titel)
                .AddPropertiesParameter(Properties)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this system within OSWEBDB.
        /// </summary>
        /// <returns>SQL statement for deleting this system within OSWEBDB.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new CommonCommandBuilder("DELETE FROM Systems WHERE SystemNo=@systemNo")
                .AddSystemNoParameter(Nummer)
                .Build();
        }

        #endregion

        #region IMySqlDataProxyCreator<ISystemProxy> Members

        /// <summary>
        /// Creates an instance of the system data proxy with values from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="dataProvider">Data provider which supports the data reader.</param>>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the system data proxy with values from the data reader.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="dataReader"/>, <paramref name="dataProvider"/> or <paramref name="columnNameCollection"/> is null.</exception>
        public virtual ISystemProxy Create(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, params string[] columnNameCollection)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider))
                .NotNull(columnNameCollection, nameof(columnNameCollection));

            return new SystemProxy(
                GetSystemNo(dataReader, columnNameCollection[0]),
                GetTitle(dataReader, columnNameCollection[1]),
                GetProperties(dataReader, columnNameCollection[2]));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the system number from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>System number.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or when <paramref name="columnName"/> is null, empty or whitespace.</exception>
        private static int GetSystemNo(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetInt32(columnName);
        }

        /// <summary>
        /// Gets the title from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>Title.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or when <paramref name="columnName"/> is null, empty or whitespace.</exception>
        private static string GetTitle(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetString(columnName);
        }

        /// <summary>
        /// Gets the properties from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>Properties.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or when <paramref name="columnName"/> is null, empty or whitespace.</exception>
        private static int GetProperties(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.IsDBNull(dataReader.GetOrdinal(columnName)) ? 0 : dataReader.GetInt32(columnName);
        }

        #endregion
    }
}
