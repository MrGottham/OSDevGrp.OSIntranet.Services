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
    /// Data proxy for en brugers kalenderaftale.
    /// </summary>
    public class BrugeraftaleProxy : Brugeraftale, IBrugeraftaleProxy
    {
        #region Private variables

        private IDataProviderBase<MySqlCommand> _dataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Danner en data proxy for en brugers kalenderaftale.
        /// </summary>
        public BrugeraftaleProxy()
            : this(0, 0, 0)
        {
        }

        /// <summary>
        /// Danner en data proxy for en brugers kalenderaftale.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet for kalenderaftalen.</param>
        /// <param name="aftale">Unik identifikation af kalenderaftalen.</param>
        /// <param name="bruger">Unik identifikation af brugeren, som kalenderaftalen er tilknyttet</param>
        /// <param name="properties">Værdi for brugers properties for kalenderaftalen.</param>
        public BrugeraftaleProxy(int system, int aftale, int bruger, int properties = 0)
            : base(new SystemProxy(system), new AftaleProxy(system, aftale), new BrugerProxy(system, bruger), properties)
        {
            DataIsLoaded = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// System under OSWEBDB, som brugeraftalen er tilknyttet.
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

        /// <summary>
        /// Aftale, som brugeraftalen er tilknyttet.
        /// </summary>
        public override IAftale Aftale
        {
            get
            {
                if (base.Aftale is ILazyLoadable)
                {
                    if (((ILazyLoadable)base.Aftale).DataIsLoaded == false && _dataProvider != null)
                    {
                        this.SetFieldValue("_aftale", this.Get(_dataProvider, base.Aftale as AftaleProxy, MethodBase.GetCurrentMethod().Name));
                    }
                }
                return base.Aftale;
            }
        }

        /// <summary>
        /// Bruger, som brugeraftalen er tilknyttet.
        /// </summary>
        public override IBruger Bruger
        {
            get
            {
                if (base.Bruger is ILazyLoadable)
                {
                    if (((ILazyLoadable)base.Bruger).DataIsLoaded == false && _dataProvider != null)
                    {
                        this.SetFieldValue("_bruger", this.Get(_dataProvider, base.Bruger as BrugerProxy, MethodBase.GetCurrentMethod().Name));
                    }
                }
                return base.Bruger;
            }
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Returnerer unik identifikation af brugerens kalenderaftale.
        /// </summary>
        public virtual string UniqueId
        {
            get
            {
                return string.Format("{0}-{1}-{2}", System.Nummer, Aftale.Id, Bruger.Id);
            }
        }

        /// <summary>
        /// Returnerer SQL forespørgelse til fremsøgning af en given brugers kalenderaftale.
        /// </summary>
        /// <param name="queryForDataProxy">Data proxy indeholdende nødvendige værdier til fremsøgning af den givne brugers kalenderaftale.</param>
        /// <returns>SQL forespørgelse.</returns>
        public virtual string GetSqlQueryForId(IBrugeraftale queryForDataProxy)
        {
            if (queryForDataProxy == null)
            {
                throw new ArgumentNullException("queryForDataProxy");
            }
            return string.Format("SELECT SystemNo,CalId,UserId,Properties FROM Calmerge WHERE SystemNo={0} AND CalId={1} AND UserId={2}", queryForDataProxy.System.Nummer, queryForDataProxy.Aftale.Id, queryForDataProxy.Bruger.Id);
        }

        /// <summary>
        /// Returnerer SQL kommando til oprettelse af brugerens kalenderaftalen.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            return string.Format("INSERT INTO Calmerge (SystemNo,CalId,UserId,Properties) VALUES({0},{1},{2},{3})", System.Nummer, Aftale.Id, Bruger.Id, Properties);
        }

        /// <summary>
        /// Returnerer SQL kommando til opdatering af brugerens kalenderaftalen.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            return string.Format("UPDATE Calmerge SET Properties={3} WHERE SystemNo={0} AND CalId={1} AND UserId={2}", System.Nummer, Aftale.Id, Bruger.Id, Properties);
        }

        /// <summary>
        /// Returnerer SQL kommando til sletning af brugerens kalenderaftalen.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM Calmerge WHERE SystemNo={0} AND CalId={1} AND UserId={2}", System.Nummer, Aftale.Id, Bruger.Id);
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Mapning af data for en brugers kalenderaftale.
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
            this.SetFieldValue("_aftale", new AftaleProxy(mySqlDataReader.GetInt32("SystemNo"), mySqlDataReader.GetInt32("CalId")));
            this.SetFieldValue("_bruger", new BrugerProxy(mySqlDataReader.GetInt32("SystemNo"), mySqlDataReader.GetInt32("UserId")));
            Properties = mySqlDataReader.GetInt32("Properties");
            DataIsLoaded = true;

            _dataProvider = dataProvider;
        }

        /// <summary>
        /// Mapning af relationer til en brugers kalenderaftale.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlCommand> dataProvider)
        {
        }

        /// <summary>
        /// Gemmer relationer til en brugers kalenderaftale.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        /// <param name="isInserting">Angivelse af, om der indsættes eller opdateres.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlCommand> dataProvider, bool isInserting)
        {
        }

        /// <summary>
        /// Sletter relationer til en brugers kalenderaftale.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlCommand> dataProvider)
        {
        }

        /// <summary>
        /// Creates the SQL statement for getting this binding between an user and an appointment.
        /// </summary>
        /// <returns>SQL statement for getting this binding between an user and an appointment.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new MySqlCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this binding between an user and an appointment.
        /// </summary>
        /// <returns>SQL statement for inserting this binding between an user and an appointment.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new MySqlCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this binding between an user and an appointment.
        /// </summary>
        /// <returns>SQL statement for updating this binding between an user and an appointment.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new MySqlCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this binding between an user and an appointment.
        /// </summary>
        /// <returns>SQL statement for deleting this binding between an user and an appointment.</returns>
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
