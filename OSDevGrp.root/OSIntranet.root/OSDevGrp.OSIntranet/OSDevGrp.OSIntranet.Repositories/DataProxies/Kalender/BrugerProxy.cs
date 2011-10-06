using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
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

        #region IMySqlDataProxy<IBruger> Members

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
        /// Mapper data for en bruer.
        /// </summary>
        /// <param name="dataReader">Datareader.</param>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void MapData(object dataReader, IDataProviderBase dataProvider)
        {
            throw new System.NotImplementedException();
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
