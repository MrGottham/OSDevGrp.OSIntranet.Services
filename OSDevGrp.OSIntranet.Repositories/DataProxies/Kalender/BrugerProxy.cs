using System;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Kalender;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Data proxy for en kalenderbruger under OSWEBDB.
    /// </summary>
    public class BrugerProxy : Bruger, IBrugerProxy
    {
        #region Constructors

        /// <summary>
        /// Danner en data proxy for en kalenderbruger under OSWEBDB.
        /// </summary>
        public BrugerProxy()
            : this(0, 0)
        {
        }

        /// <summary>
        /// Danner en data proxy for en kalenderbruger under OSWEBDB.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet for kalenderbrugeren.</param>
        /// <param name="id">Unik identifikation af brugeren.</param>
        public BrugerProxy(int system, int id)
            : this(system, id, typeof(Bruger).Name, typeof(Bruger).Name)
        {
        }

        /// <summary>
        /// Danner en data proxy for en kalenderbruger under OSWEBDB.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet for kalenderbrugeren.</param>
        /// <param name="id">Unik identifikation af brugeren.</param>
        /// <param name="initialer">Initialer på brugeren.</param>
        /// <param name="navn">Navn på brugeren.</param>
        public BrugerProxy(int system, int id, string initialer, string navn)
            : this(new SystemProxy(system), id, initialer, navn)
        {
        }

        /// <summary>
        /// Creates an instance of a data proxy for a calender user with OSWEBDB.
        /// </summary>
        /// <param name="system">The system for for the calender user.</param>
        /// <param name="id">The calender user identifier.</param>
        /// <param name="initials">The calender user's initials.</param>
        /// <param name="name">The calender user's name.½</param>
        private BrugerProxy(ISystem system, int id, string initials, string name)
            : base(system, id, initials, name)
        {
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Returnerer unik identifikation for brugeren.
        /// </summary>
        public virtual string UniqueId => $"{System.Nummer}-{Id}";

        #endregion

        #region IDataProxyBase<MySqlDataReader, MySqlCommand> Members

        /// <summary>
        /// Mapper data for en bruger.
        /// </summary>
        /// <param name="dataReader">Datareader.</param>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider));

            System = dataProvider.Create(new SystemProxy(), dataReader, "SystemNo", "Title", "Properties");

            Id = GetUserId(dataReader, "UserId");
            UserName = GetUserName(dataReader, "UserName");
            Navn = GetName(dataReader, "Name");
            Initialer = GetInitials(dataReader, "Initials");
        }

        /// <summary>
        /// Mapper relationer til en bruger.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
        }

        /// <summary>
        /// Gemmer relationer til en bruger.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        /// <param name="isInserting">Angivelse af, om der indsættes eller opdateres.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
        }

        /// <summary>
        /// Sletter relationer til en bruger.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
        }

        /// <summary>
        /// Creates the SQL statement for getting this calender user.
        /// </summary>
        /// <returns>SQL statement for getting this calender user.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new CalenderCommandBuilder("SELECT cu.SystemNo,cu.UserId,cu.UserName,cu.Name,cu.Initials,s.Title,s.Properties FROM Calusers AS cu INNER JOIN Systems AS s ON s.SystemNo=cu.SystemNo WHERE cu.SystemNo=@systemNo AND cu.UserId=@userId")
                .AddSystemNoParameter(System.Nummer)
                .AddUserIdParameter(Id)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this calender user.
        /// </summary>
        /// <returns>SQL statement for inserting this calender user.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new CalenderCommandBuilder("INSERT INTO Calusers (SystemNo,UserId,UserName,Name,Initials) VALUES(@systemNo,@userId,@userName,@name,@initials)")
                .AddSystemNoParameter(System.Nummer)
                .AddUserIdParameter(Id)
                .AddUserNameParameter(UserName)
                .AddUserFullnameParameter(Navn)
                .AddUserInitialsParameter(Initialer)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this calender user.
        /// </summary>
        /// <returns>SQL statement for updating this calender user.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new CalenderCommandBuilder("UPDATE Calusers SET UserName=@userName,Name=@name,Initials=@initials WHERE SystemNo=@systemNo AND UserId=@userId")
                .AddSystemNoParameter(System.Nummer)
                .AddUserIdParameter(Id)
                .AddUserNameParameter(UserName)
                .AddUserFullnameParameter(Navn)
                .AddUserInitialsParameter(Initialer)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this calender user.
        /// </summary>
        /// <returns>SQL statement for deleting this calender user.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new CalenderCommandBuilder("DELETE FROM Calusers WHERE SystemNo=@systemNo AND UserId=@userId")
                .AddSystemNoParameter(System.Nummer)
                .AddUserIdParameter(Id)
                .Build();
        }

        #endregion

        #region IMySqlDataProxyCreator<IBrugerProxy> Members

        /// <summary>
        /// Creates an instance of the calender user data proxy with values from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="dataProvider">Data provider which supports the data reader.</param>>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the calender user proxy with values from the data reader.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="dataReader"/>, <paramref name="dataProvider"/> or <paramref name="columnNameCollection"/> is null.</exception>
        public virtual IBrugerProxy Create(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, params string[] columnNameCollection)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider))
                .NotNull(columnNameCollection, nameof(columnNameCollection));

            ISystemProxy system  = dataProvider.Create(new SystemProxy(), dataReader, columnNameCollection[4], columnNameCollection[5], columnNameCollection[6]);

            return new BrugerProxy(
                system,
                GetUserId(dataReader, columnNameCollection[0]),
                GetInitials(dataReader, columnNameCollection[1]),
                GetName(dataReader, columnNameCollection[2]))
            {
                UserName = GetUserName(dataReader, columnNameCollection[3])
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the user identifier from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>User identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or when <paramref name="columnName"/> is null, empty or whitespace.</exception>
        private static int GetUserId(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetInt32(columnName);
        }

        /// <summary>
        /// Gets the user name from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>User name.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or when <paramref name="columnName"/> is null, empty or whitespace.</exception>
        private static string GetUserName(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.IsDBNull(dataReader.GetOrdinal(columnName)) ? null : dataReader.GetString(columnName);
        }

        /// <summary>
        /// Gets the name from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>Name.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or when <paramref name="columnName"/> is null, empty or whitespace.</exception>
        private static string GetName(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetString(columnName);
        }

        /// <summary>
        /// Gets the initials from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>Initials.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or when <paramref name="columnName"/> is null, empty or whitespace.</exception>
        private static string GetInitials(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetString(columnName);
        }

        #endregion
    }
}
