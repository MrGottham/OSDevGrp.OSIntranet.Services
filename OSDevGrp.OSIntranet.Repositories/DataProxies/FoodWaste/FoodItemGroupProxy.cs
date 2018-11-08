using System;
using System.Collections.Generic;
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
    /// Data proxy to a given relation between a food item and a food group in the food waste domain.
    /// </summary>
    public class FoodItemGroupProxy : IdentifiableBase, IFoodItemGroupProxy
    {
        #region Constructors

        /// <summary>
        /// Creates a data proxy for a given relation between a food item and a food group in the food waste domain.
        /// </summary>
        public FoodItemGroupProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy for a given relation between a food item and a food group in the food waste domain.
        /// </summary>
        /// <param name="foodItem">Food item which should be in the relation between a food item and a food group in the food waste domain.</param>
        /// <param name="foodGroup">Food group which should be in the relation between a food item and a food group in the food waste domain.</param>
        public FoodItemGroupProxy(IFoodItem foodItem, IFoodGroup foodGroup)
        {
            ArgumentNullGuard.NotNull(foodItem, nameof(foodItem))
                .NotNull(foodGroup, nameof(foodGroup));

            FoodItem = foodItem;
            FoodGroup = foodGroup;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the food item.
        /// </summary>
        public virtual IFoodItem FoodItem { get; private set; }

        /// <summary>
        /// Gets or sets the identifier for the food item.
        /// </summary>
        public virtual Guid? FoodItemIdentifier => FoodItem?.Identifier;

        /// <summary>
        /// Gets the food group.
        /// </summary>
        public virtual IFoodGroup FoodGroup { get; private set; }

        /// <summary>
        /// Gets or sets the identifier for the food group.
        /// </summary>
        public virtual Guid? FoodGroupIdentifier => FoodGroup?.Identifier;

        /// <summary>
        /// Gets or sets whether this will be the primary food group for the food item.
        /// </summary>
        public virtual bool IsPrimary
        {
            get;
            set;
        }

        #endregion

        #region IMySqlDataProxy Members

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

            Identifier = new Guid(dataReader.GetString("FoodItemGroupIdentifier"));
            IsPrimary = Convert.ToBoolean(dataReader.GetInt32("IsPrimary"));

            FoodItem = dataProvider.Create(new FoodItemProxy(), dataReader, "FoodItemIdentifier", "FoodItemIsActive");
            FoodGroup = dataProvider.Create(new FoodGroupProxy(), dataReader, "FoodGroupIdentifier", "FoodGroupParentIdentifier", "FoodGroupIsActive");
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
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }
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
        }

        /// <summary>
        /// Creates the SQL statement for getting this relation between a food item and a food group in the food waste domain.
        /// </summary>
        /// <returns>SQL statement for getting this relation between a food item and a food group in the food waste domain.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return BuildSystemDataCommandForSelecting("WHERE fig.FoodItemGroupIdentifier=@foodItemGroupIdentifier", systemDataCommandBuilder => systemDataCommandBuilder.AddFoodItemGroupIdentifierParameter(Identifier));
        }

        /// <summary>
        /// Creates the SQL statement for inserting this relation between a food item and a food group in the food waste domain.
        /// </summary>
        /// <returns>SQL statement for inserting this relation between a food item and a food group in the food waste domain.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            if (FoodItemIdentifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, FoodItemIdentifier, "FoodItemIdentifier"));
            }
            if (FoodGroupIdentifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, FoodGroupIdentifier, "FoodGroupIdentifier"));
            }

            return new SystemDataCommandBuilder("INSERT INTO FoodItemGroups (FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary) VALUES(@foodItemGroupIdentifier,@foodItemIdentifier,@foodGroupIdentifier,@isPrimary)")
                .AddFoodItemGroupIdentifierParameter(Identifier)
                .AddFoodItemIdentifierParameter(FoodItemIdentifier)
                .AddFoodGroupIdentifierParameter(FoodGroupIdentifier)
                .AddIsPrimaryParameter(IsPrimary)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this relation between a food item and a food group in the food waste domain.
        /// </summary>
        /// <returns>SQL statement for updating this relation between a food item and a food group in the food waste domain.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            if (FoodItemIdentifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, FoodItemIdentifier, "FoodItemIdentifier"));
            }
            if (FoodGroupIdentifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, FoodGroupIdentifier, "FoodGroupIdentifier"));
            }

            return new SystemDataCommandBuilder("UPDATE FoodItemGroups SET FoodItemIdentifier=@foodItemIdentifier,FoodGroupIdentifier=@foodGroupIdentifier,IsPrimary=@isPrimary WHERE FoodItemGroupIdentifier=@foodItemGroupIdentifier")
                .AddFoodItemGroupIdentifierParameter(Identifier)
                .AddFoodItemIdentifierParameter(FoodItemIdentifier)
                .AddFoodGroupIdentifierParameter(FoodGroupIdentifier)
                .AddIsPrimaryParameter(IsPrimary)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this relation between a food item and a food group in the food waste domain.
        /// </summary>
        /// <returns>SQL statement for deleting this relation between a food item and a food group in the food waste domain.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new SystemDataCommandBuilder("DELETE FROM FoodItemGroups WHERE FoodItemGroupIdentifier=@foodItemGroupIdentifier")
                .AddFoodItemGroupIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets relations between a given food item and it's food groups in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="foodItemIdentifier">Identifier for the food item on which to get the relations between the food item and it's food groups.</param>
        /// <returns>Relations between a given food item and it's food groups in the food waste domain.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataProvider"/> is null</exception>
        internal static IEnumerable<FoodItemGroupProxy> GetFoodItemGroups(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid foodItemIdentifier)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = BuildSystemDataCommandForSelecting("WHERE fig.FoodItemIdentifier=@foodItemIdentifier", systemDataCommandBuilder => systemDataCommandBuilder.AddFoodItemIdentifierParameter(foodItemIdentifier));
                return subDataProvider.GetCollection<FoodItemGroupProxy>(command);
            }
        }

        /// <summary>
        /// Build an instance of <see cref="FoodItemGroupProxy"/>.
        /// </summary>
        /// <param name="foodItemProxy">The <see cref="IFoodItemProxy"/> included in the <see cref="FoodItemGroupProxy"/>.</param>
        /// <param name="foodGroup">The <see cref="IFoodGroup"/> include in the <see cref="FoodItemGroupProxy"/>.</param>
        /// <param name="isPrimary">Indicates whether this <see cref="FoodItemGroupProxy"/> describes the primary <param name="foodGroup"> for the <paramref name="foodItemProxy"/>.</param></param>
        /// <returns>Instance of <see cref="FoodItemGroupProxy"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="foodItemProxy"/> or <paramref name="foodGroup"/> is null.</exception>
        internal static FoodItemGroupProxy Build(IFoodItemProxy foodItemProxy, IFoodGroup foodGroup, bool isPrimary)
        {
            ArgumentNullGuard.NotNull(foodItemProxy, nameof(foodItemProxy))
                .NotNull(foodGroup, nameof(foodGroup));

            return new FoodItemGroupProxy(foodItemProxy, foodGroup)
            {
                Identifier = Guid.NewGuid(),
                IsPrimary = isPrimary
            };
        }

        /// <summary>
        /// Deletes relations between a given food item and it's food groups in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="foodItemProxy">Data proxy for the food item on which to delete the relations between the food item and it's food groups.</param>
        internal static void DeleteFoodItemGroups(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, IFoodItemProxy foodItemProxy)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider))
                .NotNull(foodItemProxy, nameof(foodItemProxy));

            if (foodItemProxy.Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemProxy.Identifier, "Identifier"));
            }

            foreach (IFoodItemGroupProxy foodItemGroupProxy in GetFoodItemGroups(dataProvider, foodItemProxy.Identifier.Value))
            {
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(foodItemGroupProxy);
                }
            }
        }

        /// <summary>
        /// Deletes relations between a given food group and it's food items in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="foodGroupProxy">Data proxy for the food group on which to delete the relations between the food group and it's food items.</param>
        internal static void DeleteFoodItemGroups(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, IFoodGroupProxy foodGroupProxy)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider))
                .NotNull(foodGroupProxy, nameof(foodGroupProxy));

            foreach (IFoodItemGroupProxy foodItemGroupProxy in GetFoodItemGroups(dataProvider, foodGroupProxy))
            {
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(foodItemGroupProxy);
                }
            }
        }

        /// <summary>
        /// Gets relations between a given food group and it's food items in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="foodGroupProxy">Data proxy for the food group on which to get the relations between the food group and it's food items.</param>
        /// <returns>Relations between a given food group and it's food items in the food waste domain.</returns>
        private static IEnumerable<FoodItemGroupProxy> GetFoodItemGroups(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, IFoodGroupProxy foodGroupProxy)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider))
                .NotNull(foodGroupProxy, nameof(foodGroupProxy));

            if (foodGroupProxy.Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroupProxy.Identifier, "Identifier"));
            }

            using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = BuildSystemDataCommandForSelecting("WHERE fig.FoodGroupIdentifier=@foodGroupIdentifier", systemDataCommandBuilder => systemDataCommandBuilder.AddFoodGroupIdentifierParameter(foodGroupProxy.Identifier.Value));
                return subDataProvider.GetCollection<FoodItemGroupProxy>(command);
            }
        }

        /// <summary>
        /// Creates a MySQL command selecting a collection of <see cref="FoodItemGroupProxy"/>.
        /// </summary>
        /// <param name="whereClause">The WHERE clause which the MySQL command should use.</param>
        /// <param name="parameterAdder">The callback to add MySQL parameters to the MySQL command.</param>
        /// <returns>MySQL command selecting a collection of <see cref="FoodItemGroupProxy"/>.</returns>
        private static MySqlCommand BuildSystemDataCommandForSelecting(string whereClause = null, Action<SystemDataCommandBuilder> parameterAdder = null)
        {
            StringBuilder selectStatementBuilder = new StringBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier");
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

        #endregion
    }
}
