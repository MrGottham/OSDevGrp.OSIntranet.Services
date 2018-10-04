using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles
{
    /// <summary>
    /// Command builder which can build command to the common repository.
    /// </summary>
    internal class CommonCommandBuilder : MySqlCommandBuilder
    {
        #region Constructor

        /// <summary>
        /// Creates a command builder which can build command to the common repository.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement for the command.</param>
        /// <param name="timeout">Wait time (in seconds) before terminating the attempt to execute a command and generating an error.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="sqlStatement"/> is null, empty or white space.</exception>
        internal CommonCommandBuilder(string sqlStatement, int timeout = 30)
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
        internal CommonCommandBuilder AddSystemNoParameter(int value)
        {
            AddSmallIntParameter("@systemNo", value, 2);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the title.
        /// </summary>
        /// <param name="value">The value for the title.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="value"/> is null, empty or white space.</exception>
        internal CommonCommandBuilder AddTitleParameter(string value)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(value, nameof(value));

            AddVarCharParameter("@title", value, 50);
            return this;
        }

        /// <summary>
        /// Adds the parameter for the properties.
        /// </summary>
        /// <param name="value">The value for the properties.</param>
        /// <returns>Command builder which can build command to the common repository.</returns>
        internal CommonCommandBuilder AddPropertiesParameter(int value)
        {
            AddSmallIntParameter("@properties", value, 5);
            return this;
        }

        #endregion
    }
}
