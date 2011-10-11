using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Kalender;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Data proxy for en kalenderaftale.
    /// </summary>
    public class AftaleProxy : Aftale, IAftaleProxy
    {
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

        #region IMySqlDataProxy<IAftale> Members

        /// <summary>
        /// Returnerer unik identifikation af kalenderaftalen.
        /// </summary>
        public virtual string UniqueId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returnerer SQL forespørgelse til fremsøgning af en given kalenderaftale.
        /// </summary>
        /// <param name="queryForDataProxy">Data proxy indeholdende nødvendige værdier til fremsøgning af den givne kalenderaftale.</param>
        /// <returns>SQL forespørgelse.</returns>
        public virtual string GetSqlQueryForId(IAftale queryForDataProxy)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returnerer SQL kommando til oprettelse af kalenderaftalen.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returnerer SQL kommando til opdatering af kalenderaftalen.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returnerer SQL kommando til sletning af kalenderaftalen.
        /// </summary>
        /// <returns>SQL kommando.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Mapper data for en kalenderaftale.
        /// </summary>
        /// <param name="dataReader">Datareader.</param>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void MapData(object dataReader, IDataProviderBase dataProvider)
        {
            throw new NotImplementedException();
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
