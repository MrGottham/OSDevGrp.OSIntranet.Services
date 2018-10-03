using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy to a given translation for a domain object.
    /// </summary>
    public class TranslationProxy : Translation, ITranslationProxy
    {
        #region Constructors

        /// <summary>
        /// Craetes a data proxy to a given translation for a domain object.
        /// </summary>
        public TranslationProxy()
        {
        }

        /// <summary>
        /// Craetes a data proxy to a given translation for a domain object.
        /// </summary>
        /// <param name="translationOfIdentifier">Identifier for the domain object which name can be translated by this object.</param>
        /// <param name="translationInfo">Translation informations used to translate the name for a domain object.</param>
        /// <param name="value">Value which is the translated name for the domain object.</param>
        public TranslationProxy(Guid translationOfIdentifier, ITranslationInfo translationInfo, string value)
            : base(translationOfIdentifier, translationInfo, value)
        {
        }

        #endregion

        #region IMySqlDataProxy

        /// <summary>
        /// Gets the unique identification for the translation.
        /// </summary>
        public virtual string UniqueId
        {
            get
            {
                if (Identifier.HasValue)
                {
                    return Identifier.Value.ToString("D").ToUpper();
                }
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }
        }

        /// <summary>
        /// Gets the SQL statement for selecting a given translation.
        /// </summary>
        /// <param name="translation">Translation for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a given translation.</returns>
        public virtual string GetSqlQueryForId(ITranslation translation)
        {
            if (translation == null)
            {
                throw new ArgumentNullException("translation");
            }
            if (translation.Identifier.HasValue)
            {
                return string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.TranslationIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier", translation.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, translation.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this translation.
        /// </summary>
        /// <returns>SQL statement to insert this translation.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            var infoIdentifier = TranslationInfo.Identifier.HasValue ? TranslationInfo.Identifier.Value : Guid.Empty;
            return string.Format("INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('{0}','{1}','{2}','{3}')", UniqueId, TranslationOfIdentifier.ToString("D").ToUpper(), infoIdentifier.ToString("D").ToUpper(), Value);
        }

        /// <summary>
        /// Gets the SQL statement to update this translation.
        /// </summary>
        /// <returns>SQL statement to update this translation.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            var infoIdentifier = TranslationInfo.Identifier.HasValue ? TranslationInfo.Identifier.Value : Guid.Empty;
            return string.Format("UPDATE Translations SET OfIdentifier='{1}',InfoIdentifier='{2}',Value='{3}' WHERE TranslationIdentifier='{0}'", UniqueId, TranslationOfIdentifier.ToString("D").ToUpper(), infoIdentifier.ToString("D").ToUpper(), Value);
        }

        /// <summary>
        /// Gets the SQL statement to delete this translation.
        /// </summary>
        /// <returns>SQL statement to delete this translation.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM Translations WHERE TranslationIdentifier='{0}'", UniqueId);
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            Identifier = new Guid(dataReader.GetString("TranslationIdentifier"));
            TranslationOfIdentifier = new Guid(dataReader.GetString("OfIdentifier"));
            TranslationInfo = new TranslationInfoProxy(dataReader.GetString("CultureName"))
            {
                Identifier = new Guid(dataReader.GetString("InfoIdentifier"))
            };
            Value = dataReader.GetString("Value");
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }
        }

        /// <summary>
        /// Creates the SQL statement for getting this translation of a domain object.
        /// </summary>
        /// <returns>SQL statement for getting this translation of a domain object.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this translation of a domain object.
        /// </summary>
        /// <returns>SQL statement for inserting this translation of a domain object.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this translation of a domain object.
        /// </summary>
        /// <returns>SQL statement for updating this translation of a domain object.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this translation of a domain object.
        /// </summary>
        /// <returns>SQL statement for deleting this translation of a domain object.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForDelete()).Build();
        }

        /// <summary>
        /// Gets the translations for a given translatable domain object in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="translationOfIdentifier">Identifier for the given domain object on which to get the translations.</param>
        /// <returns>Translations for a given translatable domain object in the food waste domain.</returns>
        internal static IEnumerable<TranslationProxy> GetDomainObjectTranslations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid translationOfIdentifier)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = new FoodWasteCommandBuilder(DataRepositoryHelper.GetSqlStatementForSelectingTranslations(translationOfIdentifier)).Build();
                return subDataProvider.GetCollection<TranslationProxy>(command);
            }
        }

        /// <summary>
        /// Deletes translations for a given translatable domain object in the food waste domain.
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="translationOfIdentifier">Identifier for the given domain object on which to delete the translations.</param>
        internal static void DeleteDomainObjectTranslations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid translationOfIdentifier)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            foreach (var translationProxy in GetDomainObjectTranslations(dataProvider, translationOfIdentifier))
            {
                using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(translationProxy);
                }
            }
        }

        #endregion
    }
}
