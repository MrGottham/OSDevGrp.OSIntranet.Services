using System;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Internal builder which can build a MySQL command for SQL statements used by the <see cref="DataProviders.FoodWasteDataProvider"/>.
    /// </summary>
    internal abstract class FoodWasteCommandBuilder : MySqlCommandBuilderBase
    {
        #region Constructor

        /// <summary>
        /// Creates an instance of the internal builder which can build a MySQL command for SQL statements used by the <see cref="DataProviders.FoodWasteDataProvider"/>.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement for the MySQL command.</param>
        /// <param name="timeout">Wait time (in seconds) before terminating the attempt to execute a command and generating an error.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="sqlStatement"/> is null, empty or white space.</exception>
        protected FoodWasteCommandBuilder(string sqlStatement, int timeout = 30)
            : base(sqlStatement, timeout)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a date parameter to the command.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        /// <param name="isNullable">Indicates whether the parameter can be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="parameterName"/> is null, empty or white space.</exception>
        protected override void AddDateTimeParameter(string parameterName, DateTime? value, bool isNullable = false)
        {
            if (value.HasValue == false)
            {
                base.AddDateTimeParameter(parameterName, null, isNullable);
                return;
            }

            if (value.Value.Kind == DateTimeKind.Utc)
            {
                base.AddDateTimeParameter(parameterName, value.Value, isNullable);
                return;
            }

            base.AddDateTimeParameter(parameterName, value.Value.ToUniversalTime(), isNullable);
        }

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
