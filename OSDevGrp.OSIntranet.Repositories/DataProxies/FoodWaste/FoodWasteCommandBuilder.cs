using System;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Internal builder which can build a MySQL command for SQL statements used by the <see cref="DataProviders.FoodWasteDataProvider"/>.
    /// </summary>
    internal class FoodWasteCommandBuilder : MySqlCommandBuilderBase
    {
        #region Constructor

        /// <summary>
        /// Creates an instance of the internal builder which can build a MySQL command for SQL statements used by the <see cref="DataProviders.FoodWasteDataProvider"/>.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement for the MySQL command.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="sqlStatement"/> is null, empty or white space.</exception>
        internal FoodWasteCommandBuilder(string sqlStatement)
            : base(sqlStatement)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an identifier parameter to the command.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        /// <param name="isNullable">Indicates whether the parameter can be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="parameterName"/> is null, empty or white space.</exception>
        protected void AddIdentifierParameter(string parameterName, Guid? value, bool isNullable = false)
        {
            AddCharParameter(parameterName, value?.ToString("D").ToUpper(), 36, isNullable);
        }

        #endregion
    }
}
