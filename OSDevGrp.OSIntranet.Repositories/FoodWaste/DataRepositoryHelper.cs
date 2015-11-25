using System;

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
        /// Gets the SQL statement for selecting the translations to a given domain object in the food waste domain.
        /// </summary>
        /// <param name="translationOfIdentifier">Identifier for the given domain object on which to get the translations.</param>
        /// <returns>SQL statement for selecting the translations to a given domain object in the food waste domain.</returns>
        public static string GetSqlStatementForSelectingTranslations(Guid translationOfIdentifier)
        {
            return string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.OfIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier ORDER BY CultureName", translationOfIdentifier.ToString("D").ToUpper());
        }

        /// <summary>
        /// Gets the SQL statement for selecting the foreign keys to a given domain object in the food waste domain.
        /// </summary>
        /// <param name="foreignKeyForIdentifier">Identifier for the given domain object on which to get the foreign keys.</param>
        /// <returns>SQL statement for selecting the foreign keys to a given domain object in the food waste domain.</returns>
        public static string GetSqlStatementForSelectingForeignKeys(Guid foreignKeyForIdentifier)
        {
            return string.Format("SELECT ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue FROM ForeignKeys WHERE ForeignKeyForIdentifier='{0}' ORDER BY DataProviderIdentifier,ForeignKeyValue", foreignKeyForIdentifier.ToString("D").ToUpper());
        }

        /// <summary>
        /// Gets the SQL value for a given DateTime value.
        /// </summary>
        /// <param name="dateTime">DateTime value for which to get the SQL value.</param>
        /// <returns>SQL value for the given DateTime value.</returns>
        public static string GetSqlValueForDateTime(DateTime dateTime)
        {
            return string.Format("'{0}'", dateTime.ToString(MySqlDateTimeFormat));
        }

        /// <summary>
        /// Gets the SQL value for a given nullable DateTime value.
        /// </summary>
        /// <param name="dateTime">Nullable DateTime value for which to get the SQL value.</param>
        /// <returns>SQL value for the given nullable DateTime value.</returns>
        public static string GetSqlValueForDateTime(DateTime? dateTime)
        {
            return dateTime.HasValue ? GetSqlValueForDateTime(dateTime.Value) : "NULL";
        }
    }
}
