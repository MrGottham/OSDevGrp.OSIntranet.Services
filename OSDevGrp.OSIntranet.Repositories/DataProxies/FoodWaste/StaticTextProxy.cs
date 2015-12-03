using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy to a given static text used by the food waste domain.
    /// </summary>
    public class StaticTextProxy : StaticText, IStaticTextProxy
    {
        #region Constructors

        /// <summary>
        /// Creates a data proxy to a given static text used by the food waste domain.
        /// </summary>
        public StaticTextProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a given static text used by the food waste domain.
        /// </summary>
        /// <param name="staticTextType">Type of the static text.</param>
        /// <param name="subjectTranslationIdentifier">Translation identifier for the subject to the static text.</param>
        /// <param name="bodyTranslationIdentifier">Translation identifier for the body to the static text.</param>
        public StaticTextProxy(StaticTextType staticTextType, Guid subjectTranslationIdentifier, Guid? bodyTranslationIdentifier = null)
            : base(staticTextType, subjectTranslationIdentifier, bodyTranslationIdentifier)
        {
        }

        #endregion


        #region IMySqlDataProxy<IStaticText>

        /// <summary>
        /// Gets the unique identification for the static text.
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
        /// Gets the SQL statement for selecting a given static text.
        /// </summary>
        /// <param name="staticText">Data provider for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a given static text.</returns>
        public virtual string GetSqlQueryForId(IStaticText staticText)
        {
            if (staticText == null)
            {
                throw new ArgumentNullException("staticText");
            }
            if (staticText.Identifier.HasValue)
            {
                return string.Format("SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts WHERE StaticTextIdentifier='{0}'", staticText.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, staticText.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this static text.
        /// </summary>
        /// <returns>SQL statement to insert this static text.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            var subjectTranslationIdentifierAsSqlValue = SubjectTranslationIdentifier.ToString("D").ToUpper();
            var bodyTranslationIdentifierAsSqlValue = BodyTranslationIdentifier.HasValue ? string.Format("'{0}'", BodyTranslationIdentifier.Value.ToString("D").ToUpper()) : "NULL";
            return string.Format("INSERT INTO StaticTexts (StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier) VALUES('{0}',{1},'{2}',{3})", UniqueId, (int)Type, subjectTranslationIdentifierAsSqlValue, bodyTranslationIdentifierAsSqlValue);
        }

        /// <summary>
        /// Gets the SQL statement to update this static text.
        /// </summary>
        /// <returns>SQL statement to update this static text.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            var subjectTranslationIdentifierAsSqlValue = SubjectTranslationIdentifier.ToString("D").ToUpper();
            var bodyTranslationIdentifierAsSqlValue = BodyTranslationIdentifier.HasValue ? string.Format("'{0}'", BodyTranslationIdentifier.Value.ToString("D").ToUpper()) : "NULL";
            return string.Format("UPDATE StaticTexts SET StaticTextType={1},SubjectTranslationIdentifier='{2}',BodyTranslationIdentifier={3} WHERE StaticTextIdentifier='{0}'", UniqueId, (int)Type, subjectTranslationIdentifierAsSqlValue, bodyTranslationIdentifierAsSqlValue);
        }

        /// <summary>
        /// Gets the SQL statement to delete this static text.
        /// </summary>
        /// <returns>SQL statement to delete this static text.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM StaticTexts WHERE StaticTextIdentifier='{0}'", UniqueId);
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(object dataReader, IDataProviderBase dataProvider)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            var mySqlDataReader = dataReader as MySqlDataReader;
            if (mySqlDataReader == null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, "dataReader", dataReader.GetType().Name));
            }

            Identifier = new Guid(mySqlDataReader.GetString("StaticTextIdentifier"));
            Type = (StaticTextType) mySqlDataReader.GetInt16("StaticTextType");
            SubjectTranslationIdentifier = new Guid(mySqlDataReader.GetString("SubjectTranslationIdentifier"));
            var bodyTranslationIdentifierColumnNo = mySqlDataReader.GetOrdinal("BodyTranslationIdentifier");
            if (mySqlDataReader.IsDBNull(bodyTranslationIdentifierColumnNo))
            {
                return;
            }
            BodyTranslationIdentifier = new Guid(mySqlDataReader.GetString(bodyTranslationIdentifierColumnNo));
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            var translations = new List<ITranslation>();
            translations.AddRange(TranslationProxy.GetDomainObjectTranslations(dataProvider, SubjectTranslationIdentifier));
            if (BodyTranslationIdentifier.HasValue)
            {
                translations.AddRange(TranslationProxy.GetDomainObjectTranslations(dataProvider, BodyTranslationIdentifier.Value));
            }
            Translations = translations;
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase dataProvider, bool isInserting)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase dataProvider)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
