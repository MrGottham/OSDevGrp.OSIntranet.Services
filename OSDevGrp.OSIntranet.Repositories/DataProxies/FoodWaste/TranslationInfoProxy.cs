using System;
using System.Globalization;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy for translation information which are used for translation.
    /// </summary>
    public class TranslationInfoProxy : TranslationInfo, ITranslationInfoProxy
    {
        #region Constructors

        /// <summary>
        /// Creates a data proxy for translation information which are used for translation.
        /// </summary>
        public TranslationInfoProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy for translation information which are used for translation.
        /// </summary>
        /// <param name="cultureName">Name for the culture on which the translation information should be based.</param>
        public TranslationInfoProxy(string cultureName) 
            : base(cultureName)
        {
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Gets the unique identification for the translation information.
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

            Identifier = GetTranslationInfoIdentifier(dataReader, "TranslationInfoIdentifier");
            CultureName = GetCultureName(dataReader, "CultureName");
            CultureInfo = new CultureInfo(CultureName);
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
        /// Creates the SQL statement for getting this translation information which are used for translation.
        /// </summary>
        /// <returns>SQL statement for getting this translation information which are used for translation.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new SystemDataCommandBuilder("SELECT TranslationInfoIdentifier,CultureName FROM TranslationInfos WHERE TranslationInfoIdentifier=@translationInfoIdentifier")
                .AddTranslationInfoIdentifierParameter(Identifier)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this translation information which are used for translation.
        /// </summary>
        /// <returns>SQL statement for inserting this translation information which are used for translation.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new SystemDataCommandBuilder("INSERT INTO TranslationInfos (TranslationInfoIdentifier,CultureName) VALUES(@translationInfoIdentifier,@cultureName)")
                .AddTranslationInfoIdentifierParameter(Identifier)
                .AddCultureNameParameter(CultureName)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this translation information which are used for translation.
        /// </summary>
        /// <returns>SQL statement for updating this translation information which are used for translation.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new SystemDataCommandBuilder("UPDATE TranslationInfos SET CultureName=@cultureName WHERE TranslationInfoIdentifier=@translationInfoIdentifier")
                .AddTranslationInfoIdentifierParameter(Identifier)
                .AddCultureNameParameter(CultureName)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this translation information which are used for translation.
        /// </summary>
        /// <returns>SQL statement for deleting this translation information which are used for translation.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new SystemDataCommandBuilder("DELETE FROM TranslationInfos WHERE TranslationInfoIdentifier=@translationInfoIdentifier")
                .AddTranslationInfoIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region IMySqlDataProxyCreator<ITranslationInfoProxy> Members

        /// <summary>
        /// Creates an instance of the translation information data proxy with values from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="dataProvider">Data provider which supports the data reader.</param>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the translation information data proxy with values from the data reader.</returns>
        public virtual ITranslationInfoProxy Create(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, params string[] columnNameCollection)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider))
                .NotNull(columnNameCollection, nameof(columnNameCollection));

            return new TranslationInfoProxy(GetCultureName(dataReader, columnNameCollection[1]))
            {
                Identifier = GetTranslationInfoIdentifier(dataReader, columnNameCollection[0])
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the translation information identifier from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The translation information identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static Guid GetTranslationInfoIdentifier(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return new Guid(dataReader.GetString(columnName));
        }

        /// <summary>
        /// Gets the culture name from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The culture name.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static string GetCultureName(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetString(columnName);
        }

        #endregion
    }
}
