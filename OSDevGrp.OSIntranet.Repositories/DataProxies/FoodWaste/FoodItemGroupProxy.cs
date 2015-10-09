using System;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy to a given relation between a food item and a food group in the food waste domain.
    /// </summary>
    public class FoodItemGroupProxy : IdentifiableBase, IFoodItemGroupProxy
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier for the food item.
        /// </summary>
        public virtual Guid FoodItemIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the identifier for the food group.
        /// </summary>
        public virtual Guid FoodGroupIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether this will be the primary food group for the food item.
        /// </summary>
        public virtual bool IsPrimary
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the food item.
        /// </summary>
        public virtual IFoodItem FoodItem
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the food group.
        /// </summary>
        public virtual IFoodGroup FoodGroup
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IMySqlDataProxy<IForeignKey>

        /// <summary>
        /// Gets the unique identification for the relation between a food item and a food group.
        /// </summary>
        public virtual string UniqueId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the SQL statement for selecting a given relation between a food item and a food group.
        /// </summary>
        /// <param name="queryForDataProxy">Relation between a food item and a food group for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a given relation between a food item and a food group.</returns>
        public virtual string GetSqlQueryForId(IFoodItemGroupProxy queryForDataProxy)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to insert this relation between a food item and a food group.
        /// </summary>
        /// <returns>SQL statement to insert this relation between a food item and a food group.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to update this relation between a food item and a food group.
        /// </summary>
        /// <returns>SQL statement to update this relation between a food item and a food group.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to delete this relation between a food item and a food group.
        /// </summary>
        /// <returns>SQL statement to delete this relation between a food item and a food group.</returns>
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
