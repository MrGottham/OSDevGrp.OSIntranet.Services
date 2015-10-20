using System;
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
    /// Data proxy to a given relation between a food item and a food group in the food waste domain.
    /// </summary>
    public class FoodItemGroupProxy : IdentifiableBase, IFoodItemGroupProxy
    {
        #region Private variables

        private IFoodItem _foodItem;
        private Guid? _foodItemIdentifier;
        private IFoodGroup _foodGroup;
        private Guid? _foodGroupIdentifier;
        private IDataProviderBase _dataProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the food item.
        /// </summary>
        public virtual IFoodItem FoodItem
        {
            get
            {
                if (_foodItemIdentifier.HasValue == false || _dataProvider == null)
                {
                    return null;
                }
                if (_foodItem != null)
                {
                    return _foodItem;
                }
                using (var subDataProvider = (IDataProviderBase) _dataProvider.Clone())
                {
                    var foodItemProxy = new FoodItemProxy
                    {
                        Identifier = _foodItemIdentifier.Value
                    };
                    _foodItem = subDataProvider.Get(foodItemProxy);
                }
                return _foodItem;
            }
        }

        /// <summary>
        /// Gets or sets the identifier for the food item.
        /// </summary>
        public virtual Guid? FoodItemIdentifier
        {
            get
            {
                return _foodItemIdentifier;
            }
            set
            {
                if (_foodItemIdentifier == value)
                {
                    return;
                }
                _foodItem = null;
                _foodItemIdentifier = value;
            }
        }

        /// <summary>
        /// Gets the food group.
        /// </summary>
        public virtual IFoodGroup FoodGroup
        {
            get
            {
                if (_foodGroupIdentifier.HasValue == false || _dataProvider == null)
                {
                    return null;
                }
                if (_foodGroup != null)
                {
                    return _foodGroup;
                }
                using (var subDataProvider = (IDataProviderBase) _dataProvider.Clone())
                {
                    var foodGroupProxy = new FoodGroupProxy
                    {
                        Identifier = _foodGroupIdentifier.Value
                    };
                    _foodGroup = subDataProvider.Get(foodGroupProxy);
                }
                return _foodGroup;
            }
        }

        /// <summary>
        /// Gets or sets the identifier for the food group.
        /// </summary>
        public virtual Guid? FoodGroupIdentifier
        {
            get
            {
                return _foodGroupIdentifier;
            }
            set
            {
                if (_foodGroupIdentifier == value)
                {
                    return;
                }
                _foodGroup = null;
                _foodGroupIdentifier = value;
            }
        }

        /// <summary>
        /// Gets or sets whether this will be the primary food group for the food item.
        /// </summary>
        public virtual bool IsPrimary
        {
            get;
            set;
        }

        #endregion

        #region IMySqlDataProxy<IFoodItemGroupProxy>

        /// <summary>
        /// Gets the unique identification for the relation between a food item and a food group.
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
        /// Gets the SQL statement for selecting a given relation between a food item and a food group.
        /// </summary>
        /// <param name="foodItemGroup">Relation between a food item and a food group for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a given relation between a food item and a food group.</returns>
        public virtual string GetSqlQueryForId(IFoodItemGroupProxy foodItemGroup)
        {
            if (foodItemGroup == null)
            {
                throw new ArgumentNullException("foodItemGroup");
            }
            if (foodItemGroup.Identifier.HasValue)
            {
                return string.Format("SELECT FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary FROM FoodItemGroups WHERE FoodItemGroupIdentifier='{0}'", foodItemGroup.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemGroup.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this relation between a food item and a food group.
        /// </summary>
        /// <returns>SQL statement to insert this relation between a food item and a food group.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            if (FoodItemIdentifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, FoodItemIdentifier, "FoodItemIdentifier"));
            }
            if (FoodGroupIdentifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, FoodGroupIdentifier, "FoodGroupIdentifier"));
            }
            var foodItemIdentifier = FoodItemIdentifier.Value.ToString("D").ToUpper();
            var foodGroupIdentifier = FoodGroupIdentifier.Value.ToString("D").ToUpper();
            return string.Format("INSERT INTO FoodItemGroups (FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary) VALUES('{0}','{1}','{2}',{3})", UniqueId, foodItemIdentifier, foodGroupIdentifier, Convert.ToInt32(IsPrimary));
        }

        /// <summary>
        /// Gets the SQL statement to update this relation between a food item and a food group.
        /// </summary>
        /// <returns>SQL statement to update this relation between a food item and a food group.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            if (FoodItemIdentifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, FoodItemIdentifier, "FoodItemIdentifier"));
            }
            if (FoodGroupIdentifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, FoodGroupIdentifier, "FoodGroupIdentifier"));
            }
            var foodItemIdentifier = FoodItemIdentifier.Value.ToString("D").ToUpper();
            var foodGroupIdentifier = FoodGroupIdentifier.Value.ToString("D").ToUpper();
            return string.Format("UPDATE FoodItemGroups SET FoodItemIdentifier='{1}',FoodGroupIdentifier='{2}',IsPrimary={3} WHERE FoodItemGroupIdentifier='{0}'", UniqueId, foodItemIdentifier, foodGroupIdentifier, Convert.ToInt32(IsPrimary));
        }

        /// <summary>
        /// Gets the SQL statement to delete this relation between a food item and a food group.
        /// </summary>
        /// <returns>SQL statement to delete this relation between a food item and a food group.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM FoodItemGroups WHERE FoodItemGroupIdentifier='{0}'", UniqueId);
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

            Identifier = new Guid(mySqlDataReader.GetString("FoodItemGroupIdentifier"));
            FoodItemIdentifier = new Guid(mySqlDataReader.GetString("FoodItemIdentifier"));
            FoodGroupIdentifier = new Guid(mySqlDataReader.GetString("FoodGroupIdentifier"));
            IsPrimary = Convert.ToBoolean(mySqlDataReader.GetInt32("IsPrimary"));

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }
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

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
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

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }
        }

        #endregion
    }
}
