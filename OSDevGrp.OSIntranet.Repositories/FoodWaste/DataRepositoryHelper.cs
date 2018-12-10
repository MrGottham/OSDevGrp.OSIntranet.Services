﻿using System;
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
    }
}
