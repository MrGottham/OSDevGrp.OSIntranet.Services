namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Internal builder which can build a MySQL commands for SQL statements used by the <see cref="DataProviders.FoodWasteDataProvider"/>.
    /// </summary>
    internal class FoodWasteCommandBuilder : MySqlCommandBuilder
    {
        #region Constructor

        /// <summary>
        /// Creates an instance of the internal builder which can build a MySQL commands for SQL statements used by the <see cref="DataProviders.FoodWasteDataProvider"/>.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement for the MySQL command.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="sqlStatement"/> is null, empty or white space.</exception>
        internal FoodWasteCommandBuilder(string sqlStatement)
            : base(sqlStatement)
        {
        }

        #endregion
    }
}
