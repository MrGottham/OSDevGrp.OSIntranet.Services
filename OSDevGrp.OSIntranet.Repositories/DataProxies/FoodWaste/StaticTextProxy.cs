using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
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

        #region IMySqlDataProxy Members

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

        #endregion

        #region IDataProxyBase<MySqlDataReader, MySqlCommand> Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider));

            Identifier = new Guid(dataReader.GetString("StaticTextIdentifier"));
            Type = (StaticTextType) dataReader.GetInt16("StaticTextType");
            SubjectTranslationIdentifier = new Guid(dataReader.GetString("SubjectTranslationIdentifier"));

            if (dataReader.IsDBNull(dataReader.GetOrdinal("BodyTranslationIdentifier")))
            {
                return;
            }
            BodyTranslationIdentifier = new Guid(dataReader.GetString("BodyTranslationIdentifier"));
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            List<ITranslation> translationCollection = new List<ITranslation>(TranslationProxy.GetDomainObjectTranslations(dataProvider, SubjectTranslationIdentifier));
            if (BodyTranslationIdentifier.HasValue)
            {
                translationCollection.AddRange(TranslationProxy.GetDomainObjectTranslations(dataProvider, BodyTranslationIdentifier.Value));
            }
            Translations = translationCollection;
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Creates the SQL statement for getting this static text used by the food waste domain.
        /// </summary>
        /// <returns>SQL statement for getting this static text used by the food waste domain.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new SystemDataCommandBuilder("SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts WHERE StaticTextIdentifier=@staticTextIdentifier")
                .AddStaticTextIdentifierParameter(Identifier)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this static text used by the food waste domain.
        /// </summary>
        /// <returns>SQL statement for inserting this static text used by the food waste domain.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new SystemDataCommandBuilder("INSERT INTO StaticTexts (StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier) VALUES(@staticTextIdentifier,@staticTextType,@subjectTranslationIdentifier,@bodyTranslationIdentifier)")
                .AddStaticTextIdentifierParameter(Identifier)
                .AddStaticTextTypeIdentifierParameter(Type)
                .AddSubjectTranslationIdentifierParameter(SubjectTranslationIdentifier)
                .AddBodyTranslationIdentifierParameter(BodyTranslationIdentifier)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this static text used by the food waste domain.
        /// </summary>
        /// <returns>SQL statement for updating this static text used by the food waste domain.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new SystemDataCommandBuilder("UPDATE StaticTexts SET StaticTextType=@staticTextType,SubjectTranslationIdentifier=@subjectTranslationIdentifier,BodyTranslationIdentifier=@bodyTranslationIdentifier WHERE StaticTextIdentifier=@staticTextIdentifier")
                .AddStaticTextIdentifierParameter(Identifier)
                .AddStaticTextTypeIdentifierParameter(Type)
                .AddSubjectTranslationIdentifierParameter(SubjectTranslationIdentifier)
                .AddBodyTranslationIdentifierParameter(BodyTranslationIdentifier)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this static text used by the food waste domain.
        /// </summary>
        /// <returns>SQL statement for deleting this static text used by the food waste domain.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new SystemDataCommandBuilder("DELETE FROM StaticTexts WHERE StaticTextIdentifier=@staticTextIdentifier")
                .AddStaticTextIdentifierParameter(Identifier)
                .Build();
        }

        #endregion
    }
}
