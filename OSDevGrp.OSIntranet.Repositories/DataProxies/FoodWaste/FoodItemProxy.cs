using System;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy for a food item.
    /// </summary>
    public class FoodItemProxy : FoodItem, IFoodItemProxy
    {
        #region Constructor

        /// <summary>
        /// Creates a data proxy for a food item.
        /// </summary>
        public FoodItemProxy()
        {
        }

        #endregion

        #region IMySqlDataProxy<IFoodItem>

        /// <summary>
        /// Gets the unique identification for the food item.
        /// </summary>
        public virtual string UniqueId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the SQL statement for selecting a food item.
        /// </summary>
        /// <param name="queryForDataProxy">Food item for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a food item.</returns>
        public virtual string GetSqlQueryForId(IFoodItem queryForDataProxy)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to insert this food item.
        /// </summary>
        /// <returns>SQL statement to insert this food item</returns>
        public virtual string GetSqlCommandForInsert()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to update this food item.
        /// </summary>
        /// <returns>SQL statement to update this food item.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to delete this food item.
        /// </summary>
        /// <returns>SQL statement to delete this food item.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase dataProvider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase dataProvider, bool isInserting)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase dataProvider)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
