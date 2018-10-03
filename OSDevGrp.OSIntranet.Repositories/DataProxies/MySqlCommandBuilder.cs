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
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="sqlStatement"/> is null, empty or white space.</exception>
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

        #endregion
    }
}
