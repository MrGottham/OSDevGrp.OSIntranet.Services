using System;
using System.Collections.Generic;
using System.Reflection;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Kalender;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Data proxy for en kalenderaftale.
    /// </summary>
    public class AftaleProxy : Aftale, IAftaleProxy
    {
        #region Private variables

        private IMySqlDataProvider _dataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Danner en data proxy for en kalenderaftale.
        /// </summary>
        public AftaleProxy() : this(0, 0)
        {
        }

        /// <summary>
        /// Danner en data proxy for en kalenderaftale.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet for kalenderaftalen.</param>
        /// <param name="id">Unik identifikation af kalenderaftalen.</param>
        public AftaleProxy(int system, int id)
            : this(system, id, DateTime.MinValue, DateTime.MaxValue, typeof(Aftale).ToString())
        {
        }

        /// <summary>
        /// Danner en data proxy for en kalenderaftale.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet for kalenderaftalen.</param>
        /// <param name="id">Unik identifikation af kalenderaftalen.</param>
        /// <param name="fraTidspunkt">Fra dato og tidspunkt.</param>
        /// <param name="tilTidspunkt">Til dato og tidspunkt.</param>
        /// <param name="emne">Emne.</param>
        /// <param name="properties">Værdi for kalenderaftalens properties.</param>
        public AftaleProxy(int system, int id, DateTime fraTidspunkt, DateTime tilTidspunkt, string emne, int properties = 0)
            : base(new SystemProxy(system), id, fraTidspunkt, tilTidspunkt, emne, properties)
        {
            DataIsLoaded = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// System under OSWEBDB, som aftalen er tilknyttet.
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
        /// Returnerer unik identifikation af kalenderaftalen.
        /// </summary>
        public virtual string UniqueId
        {
            get
            {
                return string.Format("{0}-{1}", System.Nummer, Id);
            }
        }

        /// <summary>
        /// Returnerer SQL forespørgelse til fremsøgning af en given kalenderaftale.
        /// </summary>
        /// <param name="queryForDataProxy">Data proxy indeholdende nødvendige værdier til fremsøgning af den givne kalenderaftale.</param>
        /// <returns>SQL forespørgelse.</returns>
        public virtual string GetSqlQueryForId(IAftale queryForDataProxy)
        {
            if (queryForDataProxy == null)
            {
                throw new ArgumentNullException("queryForDataProxy");
            }
            return string.Format("SELECT SystemNo,CalId,Date,FromTime,ToTime,Properties,Subject,Note FROM Calapps WHERE SystemNo={0} AND CalId={1}", queryForDataProxy.System.Nummer, queryForDataProxy.Id);
        }

        /// <summary>
        /// Returnerer SQL kommando til oprettelse af kalenderaftalen.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            return string.Format("INSERT INTO Calapps (SystemNo,CalId,Date,FromTime,ToTime,Properties,Subject,Note) VALUES({0},{1},{2},{3},{4},{5},{6},{7})", System.Nummer, Id, this.GetNullableSqlString(FraTidspunkt.ToString("yyyy-MM-dd")), this.GetNullableSqlString(FraTidspunkt.ToString("HH:mm:ss")), this.GetNullableSqlString(TilTidspunkt.ToString("HH:mm:ss")), Properties, this.GetNullableSqlString(Emne), this.GetNullableSqlString(Notat));
        }

        /// <summary>
        /// Returnerer SQL kommando til opdatering af kalenderaftalen.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            return string.Format("UPDATE Calapps SET Date={2},FromTime={3},ToTime={4},Properties={5},Subject={6},Note={7} WHERE SystemNo={0} AND CalId={1}", System.Nummer, Id, this.GetNullableSqlString(FraTidspunkt.ToString("yyyy-MM-dd")), this.GetNullableSqlString(FraTidspunkt.ToString("HH:mm:ss")), this.GetNullableSqlString(TilTidspunkt.ToString("HH:mm:ss")), Properties, this.GetNullableSqlString(Emne), this.GetNullableSqlString(Notat));
        }

        /// <summary>
        /// Returnerer SQL kommando til sletning af kalenderaftalen.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM Calapps WHERE SystemNo={0} AND CalId={1}", System.Nummer, Id);
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Mapper data for en kalenderaftale.
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

            this.SetFieldValue("_system", new SystemProxy(dataReader.GetInt32("SystemNo")));
            this.SetFieldValue("_id", dataReader.GetInt32("CalId"));
            var mySqlDateTime = dataReader.GetMySqlDateTime("Date");
            var mySqlFromTime = dataReader.GetTimeSpan("FromTime");
            var mySqlToTime = dataReader.GetTimeSpan("ToTime");
            FraTidspunkt = new DateTime(mySqlDateTime.Year, mySqlDateTime.Month, mySqlDateTime.Day, mySqlFromTime.Hours,
                                        mySqlFromTime.Minutes, mySqlFromTime.Seconds);
            TilTidspunkt = new DateTime(mySqlDateTime.Year, mySqlDateTime.Month, mySqlDateTime.Day, mySqlToTime.Hours,
                                        mySqlToTime.Minutes, mySqlToTime.Seconds);
            Properties = dataReader.GetInt32("Properties");
            Emne = dataReader.GetString("Subject");
            Notat = dataReader.GetString("Note");
            var deltagere = new List<IBrugeraftale>();
            using (var clonedDataProvider = (IMySqlDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = new MySqlCommandBuilder(string.Format("SELECT SystemNo,CalId,UserId,Properties FROM Calmerge WHERE SystemNo={0} AND CalId={1} ORDER BY UserId", dataReader.GetInt32("SystemNo"), dataReader.GetInt32("CalId"))).Build();
                deltagere.AddRange(clonedDataProvider.GetCollection<BrugeraftaleProxy>(command));
            }
            this.SetFieldValue("_deltagere", deltagere);
            DataIsLoaded = true;

            _dataProvider = (IMySqlDataProvider) dataProvider;
        }

        /// <summary>
        /// Mapper relationer til en kalenderaftale.
        /// </summary>
        /// <param name="dataProviderBase">Dataprovider.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProviderBase)
        {
        }

        /// <summary>
        /// Gemmer relationer til en kalenderaftale.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        /// <param name="isInserting">Angivelse af, om der indsættes eller opdateres.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
        }

        /// <summary>
        /// Sletter relationer til en kalenderaftale.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
        }

        /// <summary>
        /// Creates the SQL statement for getting this appointment.
        /// </summary>
        /// <returns>SQL statement for getting this appointment.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new MySqlCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this appointment.
        /// </summary>
        /// <returns>SQL statement for inserting this appointment.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new MySqlCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this appointment.
        /// </summary>
        /// <returns>SQL statement for updating this appointment.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new MySqlCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this appointment.
        /// </summary>
        /// <returns>SQL statement for deleting this appointment.</returns>
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
