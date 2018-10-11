using System;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Internal builder which can build a MySQL command for SQL statements used by the <see cref="DataProviders.FoodWasteDataProvider"/> and the <see cref="Repositories.FoodWaste.SystemDataRepository"/>.
    /// </summary>
    internal class SystemDataCommandBuilder : FoodWasteCommandBuilder
    {
        #region Constructor

        /// <summary>
        /// Creates an instance of the internal builder which can build a MySQL command for SQL statements used by the <see cref="DataProviders.FoodWasteDataProvider"/> and the <see cref="Repositories.FoodWaste.SystemDataRepository"/>.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement for the MySQL command.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="sqlStatement"/> is null, empty or white space.</exception>
        internal SystemDataCommandBuilder(string sqlStatement)
            : base(sqlStatement)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a data provider identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the data provider identifier.</param>
        internal SystemDataCommandBuilder AddDataProviderIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@dataProviderIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a data provider name parameter to the command.
        /// </summary>
        /// <param name="value">The value for the data provider name.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null, empty or white space.</exception>
        internal SystemDataCommandBuilder AddDataProviderNameParameter(string value)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(value, nameof(value));

            AddVarCharParameter("@name", value, 256);
            return this;
        }

        /// <summary>
        /// Adds a handles payments parameter to the command.
        /// </summary>
        /// <param name="value">The value for the handles payments.</param>
        internal SystemDataCommandBuilder AddHandlesPaymentsParameter(bool? value)
        {
            AddBitParameter("@handlesPayments", value);
            return this;
        }

        /// <summary>
        /// Adds a data source statement identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the data source statement identifier.</param>
        internal SystemDataCommandBuilder AddDataSourceStatementIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@dataSourceStatementIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a translation identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the translation identifier.</param>
        internal SystemDataCommandBuilder AddTranslationIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@translationIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a translation of identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the translation of identifier.</param>
        internal SystemDataCommandBuilder AddTranslationOfIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@ofIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a translation value parameter to the command.
        /// </summary>
        /// <param name="value">The value for the translation value.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null, empty or white space.</exception>
        internal SystemDataCommandBuilder AddTranslationValueParameter(string value)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(value, nameof(value));

            AddVarCharParameter("@value", value, 4096);
            return this;
        }

        /// <summary>
        /// Adds a translation information identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the translation information identifier.</param>
        internal SystemDataCommandBuilder AddTranslationInfoIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@translationInfoIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a culture name parameter to the command.
        /// </summary>
        /// <param name="value">The value for the culture name.</param>
        internal SystemDataCommandBuilder AddCultureNameParameter(string value)
        {
            AddCharParameter("@cultureName", value, 5);
            return this;
        }

        #endregion
    }
}
