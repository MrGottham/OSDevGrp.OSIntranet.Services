using System;
using System.Reflection;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Resources;
using MySql.Data.MySqlClient;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Data proxy for en kalenderbruger under OSWEBDB.
    /// </summary>
    public class BrugerProxy : Bruger, IBrugerProxy
    {
        #region Private variables

        private IDataProviderBase<MySqlCommand> _dataProvider;

        #endregion

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
            : this(system, id, typeof(Bruger).ToString(), typeof(Bruger).ToString())
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
            : base(new SystemProxy(system), id, initialer, navn)
        {
            DataIsLoaded = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// System under OSWEBDB, som brugeren er tilknyttet.
        /// </summary>
        public override ISystem System
        {
            get
            {
                if (base.System is ILazyLoadable)
                {
                    if (((ILazyLoadable)base.System).DataIsLoaded == false && _dataProvider != null)
                    {
                        this.SetFieldValue("_system", this.Get(_dataProvider, base.System as SystemProxy, MethodBase.GetCurrentMethod().Name));
                    }
                }
                return base.System;
            }
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Returnerer unik identifikation for brugeren.
        /// </summary>
        public virtual string UniqueId
        {
            get
            {
                return string.Format("{0}-{1}", System.Nummer, Id);
            }
        }

        /// <summary>
        /// Returnerer SQL foresprøgelse til foresprøgelse efter brugeren.
        /// </summary>
        /// <param name="queryForDataProxy">Data proxy indeholdende nødvendige data til forespørgelsen.</param>
        /// <returns>SQL foresprøgelse.</returns>
        public virtual string GetSqlQueryForId(IBruger queryForDataProxy)
        {
            if (queryForDataProxy == null)
            {
                throw new ArgumentNullException("queryForDataProxy");
            }
            return string.Format("SELECT SystemNo,UserId,UserName,Name,Initials FROM Calusers WHERE SystemNo={0} AND UserId={1}", System.Nummer, Id);
        }

        /// <summary>
        /// Returnerer SQL kommando til oprettelse af brugeren.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            return string.Format("INSERT INTO Calusers (SystemNo,UserId,UserName,Name,Initials) VALUES({0},{1},{2},{3},{4})", System.Nummer, Id, this.GetNullableSqlString(UserName), this.GetNullableSqlString(Navn), this.GetNullableSqlString(Initialer));
        }

        /// <summary>
        /// Returnerer SQL kommando til opdatering af brugeren.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            return string.Format("UPDATE Calusers SET UserName={2},Name={3},Initials={4} WHERE SystemNo={0} AND UserId={1}", System.Nummer, Id, this.GetNullableSqlString(UserName), this.GetNullableSqlString(Navn), this.GetNullableSqlString(Initialer));
        }

        /// <summary>
        /// Returnerer SQL kommando til sletning af brugeren.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM Calusers WHERE SystemNo={0} AND UserId={1}", System.Nummer, Id);
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Mapper data for en bruger.
        /// </summary>
        /// <param name="dataReader">Datareader.</param>
        /// <param name="dataProvider">Dataprovider.</param>
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
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue,
                                                                                   dataReader.GetType(), "dataReader"));
            }

            this.SetFieldValue("_system", new SystemProxy(mySqlDataReader.GetInt32("SystemNo")));
            this.SetFieldValue("_id", mySqlDataReader.GetInt32("UserId"));
            UserName = mySqlDataReader.GetString("UserName");
            Navn = mySqlDataReader.GetString("Name");
            Initialer = mySqlDataReader.GetString("Initials");
            DataIsLoaded = true;

            _dataProvider = dataProvider;
        }

        /// <summary>
        /// Mapper relationer til en bruger.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlCommand> dataProvider)
        {
        }

        /// <summary>
        /// Gemmer relationer til en bruger.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        /// <param name="isInserting">Angivelse af, om der indsættes eller opdateres.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlCommand> dataProvider, bool isInserting)
        {
        }

        /// <summary>
        /// Sletter relationer til en bruger.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlCommand> dataProvider)
        {
        }

        /// <summary>
        /// Creates the SQL statement for getting this calender user.
        /// </summary>
        /// <returns>SQL statement for getting this calender user.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new MySqlCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this calender user.
        /// </summary>
        /// <returns>SQL statement for inserting this calender user.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new MySqlCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this calender user.
        /// </summary>
        /// <returns>SQL statement for updating this calender user.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new MySqlCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this calender user.
        /// </summary>
        /// <returns>SQL statement for deleting this calender user.</returns>
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
