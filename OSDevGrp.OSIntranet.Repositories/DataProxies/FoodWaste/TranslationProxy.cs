using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
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
        #region Private constants

        private const string CacheName = "OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste.TranslationProxy.Cache";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a data proxy to a given translation for a domain object.
        /// </summary>
        public TranslationProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a given translation for a domain object.
        /// </summary>
        /// <param name="translationOfIdentifier">Identifier for the domain object which name can be translated by this object.</param>
        /// <param name="translationInfo">Translation information used to translate the name for a domain object.</param>
        /// <param name="value">Value which is the translated name for the domain object.</param>
        public TranslationProxy(Guid translationOfIdentifier, ITranslationInfo translationInfo, string value)
            : base(translationOfIdentifier, translationInfo, value)
        {
        }

        #endregion

        #region IMySqlDataProxy Members

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

            Identifier = new Guid(dataReader.GetString("TranslationIdentifier"));
            TranslationOfIdentifier = new Guid(dataReader.GetString("OfIdentifier"));
            TranslationInfo = dataProvider.Create(new TranslationInfoProxy(), dataReader, "InfoIdentifier", "CultureName");
            Value = dataReader.GetString("Value");
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            DataProxyCache.AddDataProxyCollectionToCache(CacheName, this, translationProxy => translationProxy.Identifier == Identifier.Value);
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            DataProxyCache.RemoveDataProxyCollectionToCache(CacheName, this, translationProxy => translationProxy.Identifier == Identifier.Value);
        }

        /// <summary>
        /// Creates the SQL statement for getting this translation of a domain object.
        /// </summary>
        /// <returns>SQL statement for getting this translation of a domain object.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return BuildSystemDataCommandForSelecting("WHERE t.TranslationIdentifier=@translationIdentifier", systemDataCommandBuilder => systemDataCommandBuilder.AddTranslationIdentifierParameter(Identifier));
        }

        /// <summary>
        /// Creates the SQL statement for inserting this translation of a domain object.
        /// </summary>
        /// <returns>SQL statement for inserting this translation of a domain object.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new SystemDataCommandBuilder("INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES(@translationIdentifier,@ofIdentifier,@translationInfoIdentifier,@value)")
                .AddTranslationIdentifierParameter(Identifier)
                .AddTranslationOfIdentifierParameter(TranslationOfIdentifier)
                .AddTranslationInfoIdentifierParameter(TranslationInfo.Identifier)
                .AddTranslationValueParameter(Value)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this translation of a domain object.
        /// </summary>
        /// <returns>SQL statement for updating this translation of a domain object.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new SystemDataCommandBuilder("UPDATE Translations SET OfIdentifier=@ofIdentifier,InfoIdentifier=@translationInfoIdentifier,Value=@value WHERE TranslationIdentifier=@translationIdentifier")
                .AddTranslationIdentifierParameter(Identifier)
                .AddTranslationOfIdentifierParameter(TranslationOfIdentifier)
                .AddTranslationInfoIdentifierParameter(TranslationInfo.Identifier)
                .AddTranslationValueParameter(Value)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this translation of a domain object.
        /// </summary>
        /// <returns>SQL statement for deleting this translation of a domain object.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new SystemDataCommandBuilder("DELETE FROM Translations WHERE TranslationIdentifier=@translationIdentifier")
                .AddTranslationIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the translations for a given translatable domain object in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="translationOfIdentifier">Identifier for the given domain object on which to get the translations.</param>
        /// <returns>Translations for a given translatable domain object in the food waste domain.</returns>
        internal static IEnumerable<TranslationProxy> GetDomainObjectTranslations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid translationOfIdentifier)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            HashSet<TranslationProxy> cache = DataProxyCache.GetCachedDataProxyCollection<TranslationProxy>(CacheName);
            if (cache.Any(translationProxy => translationProxy.TranslationOfIdentifier == translationOfIdentifier))
            {
                return cache.Where(translationProxy => translationProxy.TranslationOfIdentifier == translationOfIdentifier).ToList();
            }

            using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                List<TranslationProxy> result = new List<TranslationProxy>(subDataProvider.GetCollection<TranslationProxy>(BuildSystemDataCommandForSelecting("WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName", systemDataCommandBuilder => systemDataCommandBuilder.AddTranslationOfIdentifierParameter(translationOfIdentifier))));
                DataProxyCache.AddDataProxyCollectionToCache(CacheName, result, translationProxy => translationProxy.TranslationOfIdentifier == translationOfIdentifier);
                return result;
            }
        }

        /// <summary>
        /// Creates a MySQL command selecting a collection of <see cref="TranslationProxy"/>.
        /// </summary>
        /// <param name="whereClause">The WHERE clause which the MySQL command should use.</param>
        /// <param name="parameterAdder">The callback to add MySQL parameters to the MySQL command.</param>
        /// <returns>MySQL command selecting a collection of <see cref="TranslationProxy"/>.</returns>
        internal static MySqlCommand BuildSystemDataCommandForSelecting(string whereClause = null, Action<SystemDataCommandBuilder> parameterAdder = null)
        {
            StringBuilder selectStatementBuilder = new StringBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier");
            if (string.IsNullOrWhiteSpace(whereClause) == false)
            {
                selectStatementBuilder.Append($" {whereClause}");
            }

            SystemDataCommandBuilder systemDataCommandBuilder = new SystemDataCommandBuilder(selectStatementBuilder.ToString());
            if (parameterAdder == null)
            {
                return systemDataCommandBuilder.Build();
            }

            parameterAdder(systemDataCommandBuilder);
            return systemDataCommandBuilder.Build();
        }

        /// <summary>
        /// Deletes translations for a given translatable domain object in the food waste domain.
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="translationOfIdentifier">Identifier for the given domain object on which to delete the translations.</param>
        internal static void DeleteDomainObjectTranslations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid translationOfIdentifier)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            foreach (ITranslationProxy translationProxy in GetDomainObjectTranslations(dataProvider, translationOfIdentifier))
            {
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(translationProxy);
                }
            }
        }

        #endregion
    }
}
