using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Resources;
using MySql.Data.MySqlClient;

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

        #region IMySqlDataProxy<ISystem> Members

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
        public virtual void MapData(object dataReader, IDataProviderBase dataProvider)
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
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue,
                                                                                   dataReader.GetType(), "dataReader"));
            }

            this.SetFieldValue("_nummer", mySqlDataReader.GetInt32("SystemNo"));
            Titel = mySqlDataReader.GetString("Title");
            Properties = mySqlDataReader.GetInt32("Properties");
            DataIsLoaded = true;
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
