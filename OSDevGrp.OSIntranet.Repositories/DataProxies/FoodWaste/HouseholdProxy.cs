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
    /// Data proxy to a household.
    /// </summary>
    public class HouseholdProxy : Household, IHouseholdProxy
    {
        #region Constructors

        /// <summary>
        /// Creates a data proxy to a household.
        /// </summary>
        public HouseholdProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a household.
        /// </summary>
        /// <param name="name">Name for the household.</param>
        /// <param name="description">Description for the household.</param>
        public HouseholdProxy(string name, string description = null)
            : base(name, description)
        {
        }

        /// <summary>
        /// Creates a household.
        /// </summary>
        /// <param name="name">Name for the household.</param>
        /// <param name="description">Description for the household.</param>
        /// <param name="creationTime">Date and time for when the household was created.</param>
        public HouseholdProxy(string name, string description, DateTime creationTime)
            : base(name, description, creationTime)
        {
        }

        #endregion

        #region IMySqlDataProxy<IHousehold>

        /// <summary>
        /// Gets the unique identification for the household.
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
        /// Gets the SQL statement for selecting a household.
        /// </summary>
        /// <param name="household">Household for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a household.</returns>
        public virtual string GetSqlQueryForId(IHousehold household)
        {
            if (household == null)
            {
                throw new ArgumentNullException("household");
            }
            if (household.Identifier.HasValue)
            {
                return string.Format("SELECT HouseholdIdentifier,Name,Descr,CreationTime FROM Households WHERE HouseholdIdentifier='{0}'", household.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, household.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this household.
        /// </summary>
        /// <returns>SQL statement to insert this household.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            var descriptionSqlValue = string.IsNullOrWhiteSpace(Description) ? "NULL" : string.Format("'{0}'", Description);
            return string.Format("INSERT INTO Households (HouseholdIdentifier,Name,Descr,CreationTime) VALUES('{0}','{1}',{2},{3})", UniqueId, Name, descriptionSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(CreationTime));
        }

        /// <summary>
        /// Gets the SQL statement to update this household.
        /// </summary>
        /// <returns>SQL statement to update this household.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            var descriptionSqlValue = string.IsNullOrWhiteSpace(Description) ? "NULL" : string.Format("'{0}'", Description);
            return string.Format("UPDATE Households SET Name='{1}',Descr={2},CreationTime={3} WHERE HouseholdIdentifier='{0}'", UniqueId, Name, descriptionSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(CreationTime));
        }

        /// <summary>
        /// Gets the SQL statement to delete this household.
        /// </summary>
        /// <returns>SQL statement to delete this household.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM Households WHERE HouseholdIdentifier='{0}'", UniqueId);
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

            Identifier = Guid.Parse(mySqlDataReader.GetString("HouseholdIdentifier"));
            Name = mySqlDataReader.GetString("Name");
            var descriptionColumnNo = mySqlDataReader.GetOrdinal("Descr");
            if (!mySqlDataReader.IsDBNull(descriptionColumnNo))
            {
                Description = mySqlDataReader.GetString(descriptionColumnNo);
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
