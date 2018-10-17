using System;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.FoodWaste
{
    /// <summary>
    /// Helper which contains static repository methods for the food waste domain.
    /// </summary>
    public static class DataRepositoryHelper
    {
        /// <summary>
        /// MySQL DateTime format.
        /// </summary>
        public const string MySqlDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Gets the SQL command for selecting the translations to a given domain object in the food waste domain.
        /// </summary>
        /// <param name="translationOfIdentifier">Identifier for the given domain object on which to get the translations.</param>
        /// <returns>SQL command for selecting the translations to a given domain object in the food waste domain.</returns>
        public static MySqlCommand GetSqlCommandForSelectingTranslations(Guid translationOfIdentifier)
        {
            return new SystemDataCommandBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddTranslationOfIdentifierParameter(translationOfIdentifier)
                .Build();
        }

        /// <summary>
        /// Gets the SQL command for selecting the foreign keys to a given domain object in the food waste domain.
        /// </summary>
        /// <param name="foreignKeyForIdentifier">Identifier for the given domain object on which to get the foreign keys.</param>
        /// <returns>SQL command for selecting the foreign keys to a given domain object in the food waste domain.</returns>
        public static MySqlCommand GetSqlCommandForSelectingForeignKeys(Guid foreignKeyForIdentifier)
        {
            return new SystemDataCommandBuilder("SELECT fk.ForeignKeyIdentifier,fk.DataProviderIdentifier,dp.Name AS DataProviderName,dp.HandlesPayments,dp.DataSourceStatementIdentifier,fk.ForeignKeyForIdentifier,fk.ForeignKeyForTypes,fk.ForeignKeyValue FROM ForeignKeys AS fk INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=fk.DataProviderIdentifier WHERE fk.ForeignKeyForIdentifier=@foreignKeyForIdentifier")
                .AddForeignKeyForIdentifierParameter(foreignKeyForIdentifier)
                .Build();
        }

        /// <summary>
        /// Gets the SQL value for a given DateTime value.
        /// </summary>
        /// <param name="dateTime">DateTime value for which to get the SQL value.</param>
        /// <param name="storeAsUniversalTime">Indicates whether the time should be stored as universal time.</param>
        /// <returns>SQL value for the given DateTime value.</returns>
        public static string GetSqlValueForDateTime(DateTime dateTime, bool storeAsUniversalTime = true)
        {
            var valueToStore = storeAsUniversalTime ? dateTime.ToUniversalTime() : dateTime;
            return string.Format("'{0}'", valueToStore.ToString(MySqlDateTimeFormat));
        }

        /// <summary>
        /// Gets the SQL value for a given nullable DateTime value.
        /// </summary>
        /// <param name="dateTime">Nullable DateTime value for which to get the SQL value.</param>
        /// <param name="storeAsUniversalTime">Indicates whether the time should be stored as universal time.</param>
        /// <returns>SQL value for the given nullable DateTime value.</returns>
        public static string GetSqlValueForDateTime(DateTime? dateTime, bool storeAsUniversalTime = true)
        {
            return dateTime.HasValue ? GetSqlValueForDateTime(dateTime.Value, storeAsUniversalTime) : "NULL";
        }
    }
}
