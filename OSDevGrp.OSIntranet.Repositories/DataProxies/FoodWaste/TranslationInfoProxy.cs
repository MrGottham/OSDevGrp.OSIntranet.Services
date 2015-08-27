using System;
using System.Globalization;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
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

        #region IMySqlDataProxy<ITranslationInfo> Members

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

        /// <summary>
        /// Gets the SQL statement for selecting a given translation information.
        /// </summary>
        /// <param name="translationInfo">Translation information for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a given translation information.</returns>
        public virtual string GetSqlQueryForId(ITranslationInfo translationInfo)
        {
            if (translationInfo == null)
            {
                throw new ArgumentNullException("translationInfo");
            }
            if (translationInfo.Identifier.HasValue)
            {
                return string.Format("SELECT TranslationInfoIdentifier,CultureName FROM TranslationInfos WHERE TranslationInfoIdentifier='{0}'", translationInfo.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, translationInfo.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this translation information.
        /// </summary>
        /// <returns>SQL statement to insert this translation information.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            return string.Format("INSERT INTO TranslationInfos (TranslationInfoIdentifier,CultureName) VALUES('{0}','{1}')", UniqueId, CultureName);
        }

        /// <summary>
        /// Gets the SQL statement to update this translation information.
        /// </summary>
        /// <returns>SQL statement to update this translation information.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            return string.Format("UPDATE TranslationInfos SET CultureName='{1}' WHERE TranslationInfoIdentifier='{0}'", UniqueId, CultureName);
        }

        /// <summary>
        /// Gets the SQL statement to delete this translation information.
        /// </summary>
        /// <returns>SQL statement to delete this translation information.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM TranslationInfos WHERE TranslationInfoIdentifier='{0}'", UniqueId);
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
            
            Identifier = new Guid(mySqlDataReader.GetString("TranslationInfoIdentifier"));
            CultureName = mySqlDataReader.GetString("CultureName");
            CultureInfo = new CultureInfo(CultureName);
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase dataProvider)
        {
        }

        #endregion
    }
}
