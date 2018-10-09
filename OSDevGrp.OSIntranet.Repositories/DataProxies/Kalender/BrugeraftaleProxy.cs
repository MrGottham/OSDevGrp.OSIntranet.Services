using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
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
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Returnerer unik identifikation af brugerens kalenderaftale.
        /// </summary>
        public virtual string UniqueId => $"{System.Nummer}-{Aftale.Id}-{Bruger.Id}";

        #endregion

        #region IDataProxyBase<MySqlDataReader, MySqlCommand> Members

        /// <summary>
        /// Mapning af data for en brugers kalenderaftale.
        /// </summary>
        /// <param name="dataReader">Datareader.</param>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider));

            System = dataProvider.Create(new SystemProxy(), dataReader, "SystemNo", "SystemTitle", "SystemProperties");
            Aftale = dataProvider.Create(new AftaleProxy(), dataReader, "CalId", "Date", "FromTime", "ToTime", "AppointmentProperties", "Subject", "Note", "SystemNo", "SystemTitle", "SystemProperties");
            Bruger = dataProvider.Create(new BrugerProxy(), dataReader, "UserId", "UserInitials", "UserFullname", "UserName", "SystemNo", "SystemTitle", "SystemProperties");
            Properties = dataReader.IsDBNull(dataReader.GetOrdinal("Properties")) ? 0 : dataReader.GetInt32("Properties");
        }

        /// <summary>
        /// Mapning af relationer til en brugers kalenderaftale.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
        }

        /// <summary>
        /// Gemmer relationer til en brugers kalenderaftale.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        /// <param name="isInserting">Angivelse af, om der indsættes eller opdateres.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
        }

        /// <summary>
        /// Sletter relationer til en brugers kalenderaftale.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
        }

        /// <summary>
        /// Creates the SQL statement for getting this binding between an user and an appointment.
        /// </summary>
        /// <returns>SQL statement for getting this binding between an user and an appointment.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new CalenderCommandBuilder("SELECT cm.SystemNo,cm.CalId,cm.UserId,cm.Properties,ca.Date,ca.FromTime,ca.ToTime,ca.Properties AS AppointmentProperties,ca.Subject,ca.Note,cu.UserName,cu.Name AS UserFullname,cu.Initials AS UserInitials,s.Title AS SystemTitle,s.Properties AS SystemProperties FROM Calmerge AS cm INNER JOIN Calapps AS ca ON ca.SystemNo=cm.SystemNo AND ca.CalId=cm.CalId INNER JOIN Calusers AS cu ON cu.SystemNo=cm.SystemNo AND cu.UserId=cm.UserId INNER JOIN Systems AS s ON s.SystemNo=cm.SystemNo WHERE cm.SystemNo=@systemNo AND cm.CalId=@calId AND cm.UserId=@userId")
                .AddSystemNoParameter(System.Nummer)
                .AddAppointmentIdParameter(Aftale.Id)
                .AddUserIdParameter(Bruger.Id)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this binding between an user and an appointment.
        /// </summary>
        /// <returns>SQL statement for inserting this binding between an user and an appointment.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new CalenderCommandBuilder("INSERT INTO Calmerge (SystemNo,CalId,UserId,Properties) VALUES(@systemNo,@calId,@userId,@properties)")
                .AddSystemNoParameter(System.Nummer)
                .AddAppointmentIdParameter(Aftale.Id)
                .AddUserIdParameter(Bruger.Id)
                .AddPropertiesParameter(Properties)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this binding between an user and an appointment.
        /// </summary>
        /// <returns>SQL statement for updating this binding between an user and an appointment.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new CalenderCommandBuilder("UPDATE Calmerge SET Properties=@properties WHERE SystemNo=@systemNo AND CalId=@calId AND UserId=@userId")
                .AddSystemNoParameter(System.Nummer)
                .AddAppointmentIdParameter(Aftale.Id)
                .AddUserIdParameter(Bruger.Id)
                .AddPropertiesParameter(Properties)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this binding between an user and an appointment.
        /// </summary>
        /// <returns>SQL statement for deleting this binding between an user and an appointment.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new CalenderCommandBuilder("DELETE FROM Calmerge WHERE SystemNo=@systemNo AND CalId=@calId AND UserId=@userId")
                .AddSystemNoParameter(System.Nummer)
                .AddAppointmentIdParameter(Aftale.Id)
                .AddUserIdParameter(Bruger.Id)
                .Build();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the appointment users for a given appointment.
        /// </summary>
        /// <param name="appointment">The appointment for which to get the appointment users.</param>
        /// <param name="dataProvider">The data provider which should be used to get the appointment users.</param>
        /// <returns>The appointment users.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="appointment"/> or <paramref name="dataProvider"/>is null.</exception>
        internal static IEnumerable<IBrugeraftaleProxy> GetAppointmentUsers(IAftaleProxy appointment, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(appointment, nameof(appointment))
                .NotNull(dataProvider, nameof(dataProvider));

            using (IMySqlDataProvider subDataProvider = (IMySqlDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = new CalenderCommandBuilder("SELECT cm.SystemNo,cm.CalId,cm.UserId,cm.Properties,ca.Date,ca.FromTime,ca.ToTime,ca.Properties AS AppointmentProperties,ca.Subject,ca.Note,cu.UserName,cu.Name AS UserFullname,cu.Initials AS UserInitials,s.Title AS SystemTitle,s.Properties AS SystemProperties FROM Calmerge AS cm INNER JOIN Calapps AS ca ON ca.SystemNo=cm.SystemNo AND ca.CalId=cm.CalId INNER JOIN Calusers AS cu ON cu.SystemNo=cm.SystemNo AND cu.UserId=cm.UserId INNER JOIN Systems AS s ON s.SystemNo=cm.SystemNo WHERE cm.SystemNo=@systemNo AND cm.CalId=@calId")
                    .AddSystemNoParameter(appointment.System.Nummer)
                    .AddAppointmentIdParameter(appointment.Id)
                    .Build();
                return subDataProvider.GetCollection<BrugeraftaleProxy>(command);
            }
        }

        #endregion
    }
}
