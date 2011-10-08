using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Kalender;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Data proxy for en brugers kalenderaftale.
    /// </summary>
    public class BrugeraftaleProxy : Brugeraftale, IBrugeraftaleProxy
    {
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

        #region IMySqlDataProxy<IBrugeraftale> Members

        /// <summary>
        /// Returnerer unik identifikation af brugerens kalenderaftale.
        /// </summary>
        public virtual string UniqueId
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Returnerer SQL forespørgelse til fremsøgning af en given brugers kalenderaftale.
        /// </summary>
        /// <param name="queryForDataProxy">Data proxy indeholdende nødvendige værdier til fremsøgning af den givne brugers kalenderaftale.</param>
        /// <returns>SQL forespørgelse.</returns>
        public virtual string GetSqlQueryForId(IBrugeraftale queryForDataProxy)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returnerer SQL kommando til oprettelse af brugerens kalenderaftalen.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returnerer SQL kommando til opdatering af brugerens kalenderaftalen.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returnerer SQL kommando til sletning af brugerens kalenderaftalen.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Mapning af data for en brugers kalenderaftale.
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
