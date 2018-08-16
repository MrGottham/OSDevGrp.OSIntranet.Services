using System.Data;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies
{
    /// <summary>
    /// Internal builder which can build a database command for SQL statements.
    /// </summary>
    /// <typeparam name="TDbCommand">Type of the database command which the builder should build.</typeparam>
    internal abstract class DbCommandBuilderBase<TDbCommand> where TDbCommand : IDbCommand
    {
        #region Methods

        /// <summary>
        /// Build the database command for the SQL statement.
        /// </summary>
        /// <returns>Database command for the SQL statement.</returns>
        internal abstract TDbCommand Build();

        #endregion
    }
}
