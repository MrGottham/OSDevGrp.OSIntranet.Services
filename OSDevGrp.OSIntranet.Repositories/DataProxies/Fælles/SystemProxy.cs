using System;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
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
            : this(nummer, typeof(Domain.Fælles.System).ToString())
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
            DataIsLoaded = false;
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Returnerer den unikke identifikation af systemet under OSWEBDB.
        /// </summary>
        public virtual string UniqueId
        {
            get
            {
                return Nummer.ToString();
            }
        }

        /// <summary>
        /// Returnerer SQL forespørgelse til foresprøgelse efter systemet under OSWEBDB.
        /// </summary>
        /// <param name="queryForDataProxy">Data proxy indeholdende nødvendige data til forespørgelsen.</param>
        /// <returns>SQL forespørgelse.</returns>
        public virtual string GetSqlQueryForId(ISystem queryForDataProxy)
        {
            if (queryForDataProxy == null)
            {
                throw new ArgumentNullException("queryForDataProxy");
            }
            return string.Format("SELECT SystemNo,Title,Properties FROM Systems WHERE SystemNo={0}", queryForDataProxy.Nummer);
        }

        /// <summary>
        /// Returnerer SQL kommando til indsættelse af systemet under OSWEBDB.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            return string.Format("INSERT INTO Systems (SystemNo,Title,Properties) VALUES({0},{1},{2})", Nummer, this.GetNullableSqlString(Titel), Properties);
        }

        /// <summary>
        /// Returnerer SQL kommando til opdatering af systemet under OSWEBDB.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            return string.Format("UPDATE Systems SET Title={1},Properties={2} WHERE SystemNo={0}", Nummer, this.GetNullableSqlString(Titel), Properties);
        }

        /// <summary>
        /// Returnerer SQL kommando til sletning af systemet under OSWEBDB.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM Systems WHERE SystemNo={0}", Nummer);
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Mapper data for et system under OSWEBDB.
        /// </summary>
        /// <param name="dataReader">Datareader.</param>
        /// <param name="dataProvider">Dataprovider.</param>
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

            this.SetFieldValue("_nummer", dataReader.GetInt32("SystemNo"));
            Titel = dataReader.GetString("Title");
            Properties = dataReader.GetInt32("Properties");
            DataIsLoaded = true;
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
            return new MySqlCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this system within OSWEBDB.
        /// </summary>
        /// <returns>SQL statement for inserting this system within OSWEBDB.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new MySqlCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this system within OSWEBDB.
        /// </summary>
        /// <returns>SQL statement for updating this system within OSWEBDB.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new MySqlCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this system within OSWEBDB.
        /// </summary>
        /// <returns>SQL statement for deleting this system within OSWEBDB.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new MySqlCommandBuilder(GetSqlCommandForDelete()).Build();
        }

        #endregion

        #region ILazyLoadable Members

        /// <summary>
        /// Angivelse af, om data er loaded.
        /// </summary>
        public bool DataIsLoaded
        {
            get;
            protected set;
        }

        #endregion
    }
}
