using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Kalender;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Data proxy for en kalenderaftale.
    /// </summary>
    public class AftaleProxy : Aftale, IAftaleProxy
    {
        #region Private variables

        private bool _appointmentUserCollectionHasBeenLoaded;
        private IMySqlDataProvider _dataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Danner en data proxy for en kalenderaftale.
        /// </summary>
        public AftaleProxy()
            : this(0, 0)
        {
        }

        /// <summary>
        /// Danner en data proxy for en kalenderaftale.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet for kalenderaftalen.</param>
        /// <param name="id">Unik identifikation af kalenderaftalen.</param>
        public AftaleProxy(int system, int id)
            : this(system, id, DateTime.MinValue, DateTime.MaxValue, typeof(Aftale).Name)
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
            : this(new SystemProxy(system), id, fraTidspunkt, tilTidspunkt, emne, properties)
        {
        }

        /// <summary>
        /// Creates an instance of the appointment data proxy.
        /// </summary>
        /// <param name="system">The system for the appointment.</param>
        /// <param name="id">The appointment identifier.</param>
        /// <param name="fromDateTime">The start date and time for the appointment.</param>
        /// <param name="toDateTime">The end date and time for the appointment.</param>
        /// <param name="subject">The subject for the appointment.</param>
        /// <param name="properties">The note for the appointment.</param>
        private AftaleProxy(ISystem system, int id, DateTime fromDateTime, DateTime toDateTime, string subject, int properties = 0) 
            : base(system, id, fromDateTime, toDateTime, subject, properties)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the appointment users.
        /// </summary>
        public override IEnumerable<IBrugeraftale> Deltagere
        {
            get
            {
                if (_appointmentUserCollectionHasBeenLoaded || _dataProvider == null)
                {
                    return base.Deltagere;
                }

                Deltagere = BrugeraftaleProxy.GetAppointmentUsers(this, _dataProvider);
                _appointmentUserCollectionHasBeenLoaded = true;

                return base.Deltagere;
            }
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Returnerer unik identifikation af kalenderaftalen.
        /// </summary>
        public virtual string UniqueId => $"{System.Nummer}-{Id}";

        #endregion

        #region IDataProxyBase<MySqlDataReader, MySqlCommand> Members

        /// <summary>
        /// Mapper data for en kalenderaftale.
        /// </summary>
        /// <param name="dataReader">Datareader.</param>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider));

            DateTime fromDate = GetAppointmentDate(dataReader, "Date");

            System = dataProvider.Create(new SystemProxy(), dataReader, "SystemNo", "SystemTitle", "SystemProperties");
            Id = GetAppointmentIdentifier(dataReader, "CalId");
            FraTidspunkt = fromDate.Add(GetAppointmentTime(dataReader, "FromTime"));
            TilTidspunkt = fromDate.Add(GetAppointmentTime(dataReader, "ToTime"));
            Properties = GetAppointmentProperties(dataReader, "Properties");
            Emne = GetAppointmentSubject(dataReader, "Subject");
            Notat = GetAppointmentNote(dataReader, "Note");

            _appointmentUserCollectionHasBeenLoaded = false;
            _dataProvider = (IMySqlDataProvider) dataProvider;
        }

        /// <summary>
        /// Mapper relationer til en kalenderaftale.
        /// </summary>
        /// <param name="dataProvider">Dataprovider.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            Deltagere = BrugeraftaleProxy.GetAppointmentUsers(this, dataProvider);

            _appointmentUserCollectionHasBeenLoaded = true;
            _dataProvider = (IMySqlDataProvider) dataProvider;
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
            return new CalenderCommandBuilder("SELECT ca.SystemNo,ca.CalId,ca.Date,ca.FromTime,ca.ToTime,ca.Properties,ca.Subject,ca.Note,s.Title AS SystemTitle,s.Properties AS SystemProperties FROM Calapps AS ca INNER JOIN Systems AS s ON s.SystemNo=ca.SystemNo WHERE ca.SystemNo=@systemNo AND ca.CalId=@calId")
                .AddSystemNoParameter(System.Nummer)
                .AddAppointmentIdParameter(Id)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this appointment.
        /// </summary>
        /// <returns>SQL statement for inserting this appointment.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new CalenderCommandBuilder("INSERT INTO Calapps (SystemNo,CalId,Date,FromTime,ToTime,Properties,Subject,Note) VALUES(@systemNo,@calId,@date,@fromTime,@toTime,@properties,@subject,@note)")
                .AddSystemNoParameter(System.Nummer)
                .AddAppointmentIdParameter(Id)
                .AddAppointmentDateParameter(FraTidspunkt)
                .AddAppointmentFromTimeParameter(FraTidspunkt)
                .AddAppointmentToTimeParameter(TilTidspunkt)
                .AddPropertiesParameter(Properties)
                .AddSubject(Emne)
                .AddNote(Notat)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this appointment.
        /// </summary>
        /// <returns>SQL statement for updating this appointment.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new CalenderCommandBuilder("UPDATE Calapps SET Date=@date,FromTime=@fromTime,ToTime=@toTime,Properties=@properties,Subject=@subject,Note=@note WHERE SystemNo=@systemNo AND CalId=@calId")
                .AddSystemNoParameter(System.Nummer)
                .AddAppointmentIdParameter(Id)
                .AddAppointmentDateParameter(FraTidspunkt)
                .AddAppointmentFromTimeParameter(FraTidspunkt)
                .AddAppointmentToTimeParameter(TilTidspunkt)
                .AddPropertiesParameter(Properties)
                .AddSubject(Emne)
                .AddNote(Notat)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this appointment.
        /// </summary>
        /// <returns>SQL statement for deleting this appointment.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new CalenderCommandBuilder("DELETE FROM Calapps WHERE SystemNo=@systemNo AND CalId=@calId")
                .AddSystemNoParameter(System.Nummer)
                .AddAppointmentIdParameter(Id)
                .Build();
        }

        #endregion

        #region IMySqlDataProxyCreator<IAftaleProxy> Members

        /// <summary>
        /// Creates an instance of the appointment data proxy with values from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="dataProvider">Data provider which supports the data reader.</param>>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the appointment proxy with values from the data reader.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="dataReader"/>, <paramref name="dataProvider"/> or <paramref name="columnNameCollection"/> is null.</exception>
        public virtual IAftaleProxy Create(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, params string[] columnNameCollection)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider))
                .NotNull(columnNameCollection, nameof(columnNameCollection));

            DateTime fromDate = GetAppointmentDate(dataReader, columnNameCollection[1]);

            ISystemProxy systemProxy = dataProvider.Create(new SystemProxy(), dataReader, columnNameCollection[7], columnNameCollection[8], columnNameCollection[9]);
            return new AftaleProxy(
                systemProxy,
                GetAppointmentIdentifier(dataReader, columnNameCollection[0]),
                fromDate.Add(GetAppointmentTime(dataReader, columnNameCollection[2])),
                fromDate.Add(GetAppointmentTime(dataReader, columnNameCollection[3])),
                GetAppointmentSubject(dataReader, columnNameCollection[5]),
                GetAppointmentProperties(dataReader, columnNameCollection[4]))
            {
                Notat = GetAppointmentNote(dataReader, columnNameCollection[6])
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the appointment identifier from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>Appointment identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or when <paramref name="columnName"/> is null, empty or whitespace.</exception>
        private static int GetAppointmentIdentifier(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetInt32(columnName);
        }

        /// <summary>
        /// Gets the appointment date from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>Appointment date.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or when <paramref name="columnName"/> is null, empty or whitespace.</exception>
        private static DateTime GetAppointmentDate(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetMySqlDateTime(columnName).GetDateTime().Date;
        }

        /// <summary>
        /// Gets an appointment time from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>Appointment time.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or when <paramref name="columnName"/> is null, empty or whitespace.</exception>
        private static TimeSpan GetAppointmentTime(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetTimeSpan(columnName);
        }

        /// <summary>
        /// Gets the appointment properties from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>Appointment properties.</returns>
        private static int GetAppointmentProperties(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.IsDBNull(dataReader.GetOrdinal(columnName)) ? 0 : dataReader.GetInt32(columnName);
        }

        /// <summary>
        /// Gets the appointment subject from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>Appointment subject.</returns>
        private static string GetAppointmentSubject(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.IsDBNull(dataReader.GetOrdinal(columnName)) ? null : dataReader.GetString(columnName);
        }

        /// <summary>
        /// Gets the appointment note from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Column name.</param>
        /// <returns>Appointment note.</returns>
        private static string GetAppointmentNote(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.IsDBNull(dataReader.GetOrdinal(columnName)) ? null : dataReader.GetString(columnName);
        }

        #endregion
    }
}
