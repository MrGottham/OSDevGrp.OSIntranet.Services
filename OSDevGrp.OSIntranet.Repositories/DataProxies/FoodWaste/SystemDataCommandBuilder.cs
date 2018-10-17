using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
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
        /// <param name="timeout">Wait time (in seconds) before terminating the attempt to execute a command and generating an error.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="sqlStatement"/> is null, empty or white space.</exception>
        internal SystemDataCommandBuilder(string sqlStatement, int timeout = 30)
            : base(sqlStatement, timeout)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a foreign key identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the foreign key identifier.</param>
        internal SystemDataCommandBuilder AddForeignKeyIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@foreignKeyIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a foreign key for identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the foreign key for identifier.</param>
        internal SystemDataCommandBuilder AddForeignKeyForIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@foreignKeyForIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a foreign key for types parameter to the command.
        /// </summary>
        /// <param name="value">The value for the foreign key for types.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        internal SystemDataCommandBuilder AddForeignKeyForTypesParameter(IEnumerable<Type> value)
        {
            ArgumentNullGuard.NotNull(value, nameof(value));

            AddVarCharParameter("@foreignKeyForTypes", string.Join(";", value.Select(m => m.Name)), 128);
            return this;
        }

        /// <summary>
        /// Adds a foreign key value parameter to the command.
        /// </summary>
        /// <param name="value">The value for the foreign key value.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null, empty or white space.</exception>
        internal SystemDataCommandBuilder AddForeignKeyValueParameter(string value)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(value, nameof(value));

            AddVarCharParameter("@foreignKeyValue", value, 128);
            return this;
        }

        /// <summary>
        /// Adds a static text identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the static text identifier.</param>
        internal SystemDataCommandBuilder AddStaticTextIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@staticTextIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a static text identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the static text identifier.</param>
        internal SystemDataCommandBuilder AddStaticTextTypeIdentifierParameter(StaticTextType value)
        {
            AddTinyIntParameter("@staticTextType", (int) value, 4);
            return this;
        }

        /// <summary>
        /// Adds a subject translation identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the subject translation identifier.</param>
        internal SystemDataCommandBuilder AddSubjectTranslationIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@subjectTranslationIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a body translation identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the body translation identifier.</param>
        internal SystemDataCommandBuilder AddBodyTranslationIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@bodyTranslationIdentifier", value, true);
            return this;
        }

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
