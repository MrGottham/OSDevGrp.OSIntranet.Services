using System;
using System.Data;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies
{
    /// <summary>
    /// Internal builder which can build a MySQL commands for SQL statements.
    /// </summary>
    internal class MySqlCommandBuilder : DbCommandBuilderBase<MySqlCommand>
    {
        #region Constructor

        /// <summary>
        /// Creates an instance of the internal builder which can build a MySQL commands for SQL statements. 
        /// </summary>
        /// <param name="sqlStatement">The SQL statement for the MySQL command.</param>
        /// <param name="timeout">Wait time (in seconds) before terminating the attempt to execute a command and generating an error.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="sqlStatement"/> is null, empty or white space.</exception>
        internal MySqlCommandBuilder(string sqlStatement, int timeout = 30)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(sqlStatement, nameof(sqlStatement));

            Command = new MySqlCommand(sqlStatement)
            {
                CommandType = CommandType.Text,
                CommandTimeout = timeout
            };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the MySQL command.
        /// </summary>
        protected MySqlCommand Command { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Build the MySQL command for the SQL statement.
        /// </summary>
        /// <returns>MySQL command for the SQL statement.</returns>
        internal override MySqlCommand Build()
        {
            return Command;
        }

        /// <summary>
        /// Adds a smallint parameter to the command.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        /// <param name="size">The size for the parameter.</param>
        /// <param name="isNullable">Indicates whether the parameter can be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="parameterName"/> is null, empty or white space.</exception>
        protected void AddSmallIntParameter(string parameterName, object value, int size = 0, bool isNullable = false)
        {
            AddParameter(parameterName, value, MySqlDbType.Int16, size, isNullable);
        }

        /// <summary>
        /// Adds a varchar parameter to the command.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        /// <param name="size">The size for the parameter.</param>
        /// <param name="isNullable">Indicates whether the parameter can be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="parameterName"/> is null, empty or white space.</exception>
        protected void AddVarCharParameter(string parameterName, object value, int size = 0, bool isNullable = false)
        {
            AddParameter(parameterName, value, MySqlDbType.VarChar, size, isNullable);
        }

        /// <summary>
        /// Adds a parameter to the command.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        /// <param name="dbType">The database type for the parameter.</param>
        /// <param name="size">The size for the parameter.</param>
        /// <param name="isNullable">Indicates whether the parameter can be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="parameterName"/> is null, empty or white space.</exception>
        private void AddParameter(string parameterName, object value, MySqlDbType dbType, int size = 0, bool isNullable = false)
        {
            MySqlParameter mySqlParameter = Command.Parameters.AddWithValue(parameterName, value);
            mySqlParameter.MySqlDbType = dbType;
            mySqlParameter.Size = size;
            mySqlParameter.IsNullable = isNullable;
        }

        #endregion
    }
}
