using System;
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
    /// Data proxy to a household member.
    /// </summary>
    public class HouseholdMemberProxy : HouseholdMember, IHouseholdMemberProxy
    {
        #region Constructors

        /// <summary>
        /// Creates a data proxy to a household member.
        /// </summary>
        public HouseholdMemberProxy() 
        {
        }

        /// <summary>
        /// Creates a data proxy to a household member.
        /// </summary>
        /// <param name="mailAddress">Mail address for the household member.</param>
        public HouseholdMemberProxy(string mailAddress)
            : base(mailAddress)
        {
        }

        /// <summary>
        /// Creates a data proxy to a household member.
        /// </summary>
        /// <param name="mailAddress">Mail address for the household member.</param>
        /// <param name="activationCode">Activation code for the household member.</param>
        /// <param name="creationTime">Date and time for when the household member was created.</param>
        public HouseholdMemberProxy(string mailAddress, string activationCode, DateTime creationTime)
            : base(mailAddress, activationCode, creationTime)
        {
        }

        #endregion

        #region IMySqlDataProxy<IFoodItem>

        /// <summary>
        /// Gets the unique identification for the household member.
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
        /// Gets the SQL statement for selecting a household member.
        /// </summary>
        /// <param name="householdMember">Household member for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a household member.</returns>
        public virtual string GetSqlQueryForId(IHouseholdMember householdMember)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }
            if (householdMember.Identifier.HasValue)
            {
                return string.Format("SELECT HouseholdMemberIdentifier,MailAddress,ActivationCode,ActivationTime,CreationTime FROM HouseholdMembers WHERE HouseholdMemberIdentifier='{0}'", householdMember.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdMember.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this household member.
        /// </summary>
        /// <returns>SQL statement to insert this household member.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            var activationTimeAsSqlValue = DataRepositoryHelper.GetSqlValueForDateTime(ActivationTime.HasValue ? ActivationTime.Value.ToUniversalTime() : (DateTime?) null);
            var creationTimeAsSqlValue = DataRepositoryHelper.GetSqlValueForDateTime(CreationTime.ToUniversalTime());
            return string.Format("INSERT INTO HouseholdMembers (HouseholdMemberIdentifier,MailAddress,ActivationCode,ActivationTime,CreationTime) VALUES('{0}','{1}','{2}',{3},{4})", UniqueId, MailAddress, ActivationCode, activationTimeAsSqlValue, creationTimeAsSqlValue);
        }

        /// <summary>
        /// Gets the SQL statement to update this household member.
        /// </summary>
        /// <returns>SQL statement to update this household member.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            var activationTimeAsSqlValue = DataRepositoryHelper.GetSqlValueForDateTime(ActivationTime.HasValue ? ActivationTime.Value.ToUniversalTime() : (DateTime?)null);
            var creationTimeAsSqlValue = DataRepositoryHelper.GetSqlValueForDateTime(CreationTime.ToUniversalTime());
            return string.Format("UPDATE HouseholdMembers SET MailAddress='{1}',ActivationCode='{2}',ActivationTime={3},CreationTime={4} WHERE HouseholdMemberIdentifier='{0}'", UniqueId, MailAddress, ActivationCode, activationTimeAsSqlValue, creationTimeAsSqlValue);
        }

        /// <summary>
        /// Gets the SQL statement to delete this household member.
        /// </summary>
        /// <returns>SQL statement to delete this household member.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM HouseholdMembers WHERE HouseholdMemberIdentifier='{0}'", UniqueId);
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

            Identifier = Guid.Parse(mySqlDataReader.GetString("HouseholdMemberIdentifier"));
            MailAddress = mySqlDataReader.GetString("MailAddress");
            ActivationCode = mySqlDataReader.GetString("ActivationCode");

            var activationTimeColumnNo = mySqlDataReader.GetOrdinal("ActivationTime");
            if (!mySqlDataReader.IsDBNull(activationTimeColumnNo))
            {
                ActivationTime = mySqlDataReader.GetDateTime(activationTimeColumnNo).ToLocalTime();
            }

            CreationTime = mySqlDataReader.GetDateTime("CreationTime").ToLocalTime();
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
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase dataProvider, bool isInserting)
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
        public virtual void DeleteRelations(IDataProviderBase dataProvider)
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

        #endregion
    }
}
