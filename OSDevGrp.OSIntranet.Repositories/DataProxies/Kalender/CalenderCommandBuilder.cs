using System;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Command builder which can build command to the calender repository.
    /// </summary>
    internal class CalenderCommandBuilder : MySqlCommandBuilderBase
    {
        #region Constructor

        /// <summary>
        /// Creates a command builder which can build command to the calender repository.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement for the command.</param>
        /// <param name="timeout">Wait time (in seconds) before terminating the attempt to execute a command and generating an error.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="sqlStatement"/> is null, empty or white space.</exception>
        internal CalenderCommandBuilder(string sqlStatement, int timeout = 30)
            : base(sqlStatement, timeout)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the parameter for the system number.
        /// </summary>
        /// <param name="value">The value for the system number.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CalenderCommandBuilder AddSystemNoParameter(int value)
        {
            AddSmallIntParameter("@systemNo", value, 2);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the appointment identifier.
        /// </summary>
        /// <param name="value">The value for the appointment identifier.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CalenderCommandBuilder AddAppointmentIdParameter(int value)
        {
            AddIntParameter("@calId", value, 8);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the appointment date.
        /// </summary>
        /// <param name="value">The value for the appointment date.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CalenderCommandBuilder AddAppointmentDateParameter(DateTime value)
        {
            AddDateParameter("@date", value);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the appointment start time.
        /// </summary>
        /// <param name="value">The value for the appointment start time.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CalenderCommandBuilder AddAppointmentFromTimeParameter(DateTime value)
        {
            AddTimeParameter("@fromTime", value);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the appointment end time.
        /// </summary>
        /// <param name="value">The value for the appointment end time.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CalenderCommandBuilder AddAppointmentToTimeParameter(DateTime value)
        {
            AddTimeParameter("@toTime", value);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the subject.
        /// </summary>
        /// <param name="value">The value for the subject.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CalenderCommandBuilder AddSubject(string value)
        {
            AddVarCharParameter("@subject", value, 255, true);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the note.
        /// </summary>
        /// <param name="value">The value for the note.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CalenderCommandBuilder AddNote(string value)
        {
            AddTextParameter("@note", value, true);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the appointment properties.
        /// </summary>
        /// <param name="value">The value for the appointment properties.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CalenderCommandBuilder AddPropertiesParameter(int value)
        {
            AddSmallIntParameter("@properties", value, 3, true);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the user identifier.
        /// </summary>
        /// <param name="value">The value for the user identifier.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CalenderCommandBuilder AddUserIdParameter(int value)
        {
            AddIntParameter("@userId", value, 8);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the user name.
        /// </summary>
        /// <param name="value">The value for the user name.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CalenderCommandBuilder AddUserNameParameter(string value)
        {
            AddVarCharParameter("@userName", value, 16, true);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the user fullname.
        /// </summary>
        /// <param name="value">The value for the user fullname.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CalenderCommandBuilder AddUserFullnameParameter(string value)
        {
            AddVarCharParameter("@name", value, 40);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the user initials.
        /// </summary>
        /// <param name="value">The value for the user initials.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CalenderCommandBuilder AddUserInitialsParameter(string value)
        {
            AddVarCharParameter("@initials", value, 8);
            return this;
        }

        #endregion
    }
}
